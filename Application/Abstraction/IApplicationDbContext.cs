using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Models.Token;

namespace Application.Abstraction;

public interface IApplicationDbContext
{
    public DbSet<Product> Products { get; }
    public DbSet<User> Users { get; }
    public DbSet<Role> Roles { get; }
    public DbSet<Permission> Permissions { get; }
    public DbSet<RolePermission> RolePermissions { get;  }
    public DbSet<UserRole> UserRoles { get; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
