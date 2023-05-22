using Application.Abstraction;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _dbContext;
    private readonly int _refreshTokenLifetime;
    private readonly int _accessTokenLifetime;
    public TokenService(IConfiguration configuration, IApplicationDbContext dbContext)
    {
        
        _configuration = configuration;
        _dbContext = dbContext;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        _refreshTokenLifetime = int.Parse(configuration["JWT:RefreshTokenLifetime"]);
        _accessTokenLifetime = int.Parse(configuration["JWT:AccessTokenLifetime"]);
    }
    
    public async Task<Tokens> CreateTokensAsync(User user)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        foreach (UserRole role in user.UserRoles)
        {
            foreach (RolePermission permission in role.Role.RolePermissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.Permission?.PermissionName));
            }
        }
        claims = claims.Distinct().ToList();

        Tokens tokens = CreateToken(claims);

        var SavedRefreshToken = Get(x => x.Username == user.Username).FirstOrDefault();
        if (SavedRefreshToken == null)
        {
            var refreshToken = new RefreshToken()
            {
                ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime),
                RefreshTokenValue = tokens.RefreshToken,
                Username = user.Username
            };
            await AddRefreshToken(refreshToken);

        }
        else
        {
            SavedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
            SavedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime);
            Update(SavedRefreshToken);
        }

        return tokens;
    }

    public Task<Tokens> CreateTokensFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken)
    {
        Tokens tokens = CreateToken(principal.Claims);

        savedRefreshToken.RefreshTokenValue = tokens.RefreshToken;
        savedRefreshToken.ExpiredDate = DateTime.UtcNow.AddMinutes(_refreshTokenLifetime);

        Update(savedRefreshToken);
        return Task.FromResult(tokens);
    }


    public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
    {
        byte[] Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

        var tokenParams = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = _configuration["JWT:Audience"],
            ValidateIssuer = true,
            ValidateLifetime = false,
            ValidIssuer = _configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };

        JwtSecurityTokenHandler tokenHandler = new();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenParams, out SecurityToken securityToken);
        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null)// ||
                                     //jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }

    public async Task<bool> AddRefreshToken(RefreshToken tokens)
    {
        await _dbContext.RefreshToken.AddAsync(tokens);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public bool Update(RefreshToken tokens)
    {
        _dbContext.RefreshToken.Update(tokens);
        _dbContext.SaveChangesAsync();
        return true;
    }

    public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> predicate)
    {
        return _dbContext.RefreshToken.Where(predicate);
    }

    public bool Delete(RefreshToken token)
    {
        _dbContext.RefreshToken.Remove(token);
        _dbContext.SaveChangesAsync();
        return true;
    }


    private Tokens CreateToken(IEnumerable<Claim> claims)
    {
        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(_accessTokenLifetime),
            claims: claims,
            signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                                        SecurityAlgorithms.HmacSha256Signature)
            );
        Tokens tokens = new()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = GenerateRefreshToken()
        };
        return tokens;
    }

    private string GenerateRefreshToken()
    {
        return (DateTime.UtcNow.ToString() + _configuration["JWT:Key"]).ComputeHash();
    }
}
