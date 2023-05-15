using Application.Abstraction;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

internal class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _DbContext;

    public UserRepository(IApplicationDbContext userRepository)
    {
        _DbContext = userRepository;
    }

    public string ComputeHash(string input)
    {
        using SHA256 sha256 = SHA256.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);
        byte[] hashBytes = sha256.ComputeHash(inputBytes);

        StringBuilder builder = new();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            builder.Append(hashBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }

    public async Task<bool> CreateAsync(User entity)
    {
        var roles = new List<UserRole>();
        foreach (var item in entity.Roles)
        {
            roles.Add(new UserRole()
            {
                Role = _DbContext.Roles.Find(item)
            });
        }
        entity.UserRoles = roles;
        entity.Password = ComputeHash(entity.Password);
        await _DbContext.Users.AddAsync(entity);
        int result = await _DbContext.SaveChangesAsync();
        return result > 0;
    }


    public Task<bool> DeleteAsync(int Id)
    {
        User? user = _DbContext.Users.FirstOrDefault(x => x.UsersId == Id);
        if (user != null)
        {
            _DbContext.Users.Remove(user);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null)
    {
        return expression == null ? Task.FromResult(_DbContext.Users.AsQueryable()) :
                                    Task.FromResult(_DbContext.Users.Where(expression));
    }

    public Task<User> GetAsync(Expression<Func<User, bool>> predicate)
    {
        User? user = _DbContext.Users.Where(predicate)?
                                    .Include(x => x.UserRoles)
                                    .ThenInclude(x => x.Role)
                                    .ThenInclude(x => x.RolePermissions)
                                    .ThenInclude(x => x.Permission)
                                    .Select(x => x).FirstOrDefault();
        return Task.FromResult(user);
    }

    public async Task<bool> UpdateAsync(User entity)
    {
        _DbContext.Users.Update(entity);
        int result = await _DbContext.SaveChangesAsync();
        return result > 0;
    }
}
