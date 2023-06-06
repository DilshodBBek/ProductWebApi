using Application.Abstraction;
using Application.Intefaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Services;

public class ProductRepository : IProductRepository
{
    private readonly IDistributedCache _distributedCache;

    private readonly IApplicationDbContext _dbContext;
    private readonly string _key = "Key";
    public ProductRepository(IApplicationDbContext db, IDistributedCache distributedCache)
    {
        _dbContext = db;
        _distributedCache = distributedCache;
    }

    public async Task<bool> CreateAsync(Product entity)
    {
        _distributedCache.Remove(_key);
        _dbContext.Products.Add(entity);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<bool> Create(Product entity)
    {
        _distributedCache.Remove(_key);
        await _dbContext.Products.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _distributedCache.Remove(_key);
        Product? product = _dbContext.Products.FirstOrDefault(p => p.ProductId == id);
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public Task<IQueryable<Product>> GetAllAsync(Expression<Func<Product, bool>>? expression = null)
    {
        var res = _distributedCache.GetStringAsync(_key);
        IQueryable<Product> queryable = expression == null ? _dbContext.Products : _dbContext.Products.Where(expression);
        _distributedCache.SetStringAsync(_key, queryable.AsEnumerable().ToString());
        return Task.FromResult(queryable);
    }

    public Task<Product?> GetAsync(Expression<Func<Product, bool>> expression)
    {

        Product? product = _dbContext.Products.FirstOrDefault(expression);

        return Task.FromResult(product);
    }

    public async Task<bool> UpdateAsync(Product entity)
    {
        _dbContext.Products.Update(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
