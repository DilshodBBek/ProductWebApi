using System.Linq.Expressions;

namespace Application.Intefaces
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int Id);

    }
}
