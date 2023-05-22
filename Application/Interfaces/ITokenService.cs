using Domain.Models;
using Domain.Models.Token;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    public Task<Tokens> CreateTokensAsync(User user);
    public Task<Tokens> CreateTokensFromRefresh(ClaimsPrincipal principal, RefreshToken savedRefreshToken);
    public ClaimsPrincipal GetClaimsFromExpiredToken(string token);

    public Task<bool> AddRefreshToken(RefreshToken tokens);
    public bool Update(RefreshToken tokens);
    public IQueryable<RefreshToken> Get(Expression<Func<RefreshToken, bool>> predicate);
    public bool Delete(RefreshToken token);

}
