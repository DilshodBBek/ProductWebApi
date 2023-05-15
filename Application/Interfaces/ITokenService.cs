using Domain.Models;
using Domain.Models.UserCredentials;

namespace Application.Interfaces;

public interface ITokenService
{
    public Task<string> CreateAccessToken(User user);

    
}
