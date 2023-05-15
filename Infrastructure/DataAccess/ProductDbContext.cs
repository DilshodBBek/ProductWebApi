using Application.Abstraction;
using Domain.Models;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().Navigation(x => x.UserRoles).AutoInclude();
        //modelBuilder.Entity<UserRole>().Navigation(x => x.Role).AutoInclude();
        //modelBuilder.Entity<Role>().Navigation(x => x.RolePermissions).AutoInclude();
        //modelBuilder.Entity<RolePermission>().Navigation(x => x.Permission).AutoInclude();
    }
}
