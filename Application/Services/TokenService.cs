using Application.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<string> CreateAccessToken(User user)
    {
        List<Claim> claims = new()
        {
            new Claim("Username", user.Username)
        };

        foreach (UserRole role in user.UserRoles)
        {
            foreach (RolePermission permission in role.Role.RolePermissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.Permission?.PermissionName));
            }
        }
        claims= claims.Distinct().ToList();

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.UtcNow.AddMinutes(5),
            claims: claims,
            signingCredentials: new SigningCredentials(
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                                        SecurityAlgorithms.HmacSha256)
            );
        return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
    }


}
