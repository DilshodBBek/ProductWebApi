using Application.Abstraction;
using Application.Intefaces;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Services;

public class ProductRepository : IProductRepository
{

    private readonly IApplicationDbContext _dbContext;

    public ProductRepository(IApplicationDbContext db)
    {
        _dbContext = db;
    }

    public async Task<bool> CreateAsync(Product entity)
    {
        _dbContext.Products.Add(entity);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<bool> Create(Product entity)
    {
        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<bool> DeleteAsync(int id)
    {
        Product? product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public Task<IQueryable<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression=null)
    {
        IQueryable<Product> queryable = _dbContext.Products;
        return Task.FromResult(queryable);
    }

    public Task<Product?> GetByIdAsync(int id)
    {

        Product? product = _dbContext.Products.FirstOrDefault(x => x.ProductId == id);
        return Task.FromResult(product);
    }

    public async Task<bool> UpdateAsync(Product entity)
    {
        _dbContext.Products.Update(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
