using Application.Abstraction;
using Application.Intefaces;
using Domain.Models;

namespace Application.Services
{
    public class ProductService : IProductService
    {

        private readonly IApplicationDbContext _db;

        public ProductService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateAsync(Product entity)
        {
            _db.Products.Add(entity);   
           await _db.SaveChangesAsync(); 
            return true;

        }

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Product>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
