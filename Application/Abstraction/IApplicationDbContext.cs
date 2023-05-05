using Microsoft.EntityFrameworkCore;
using Domain.Models;

namespace Application.Abstraction
{
    public interface IApplicationDbContext
    {
        public DbSet<Product> Products { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
