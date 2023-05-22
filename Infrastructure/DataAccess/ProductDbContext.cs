using Application.Abstraction;
using Domain.Models;
using Domain.Models.Token;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess;

public class ProductDbContext : DbContext, IApplicationDbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshToken { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(option => option.Username).IsUnique();
    }
}
