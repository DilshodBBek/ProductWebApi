using Application.Abstraction;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Application.Services;

internal class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _DbContext;

    public UserRepository(IApplicationDbContext userRepository)
    {
        _DbContext = userRepository;
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
        entity.Password = entity.Password.ComputeHash();
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
        throw new NotImplementedException("GetAllAsync exception");
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
