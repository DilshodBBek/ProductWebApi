using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Domain.Models;

namespace Infrastructure.DataAccess
{
    public class ProductDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Product> Products { get; }
    }
}
