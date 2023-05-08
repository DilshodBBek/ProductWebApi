using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Domain.Models;

namespace Infrastructure.DataAccess
{
    public class ProductDbContext : DbContext, IApplicationDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
    }
}
