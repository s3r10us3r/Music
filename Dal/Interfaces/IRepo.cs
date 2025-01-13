using System.Linq.Expressions;
using Models;

namespace Dal.Interfaces;

public interface IRepo<T> where T : Entity
{
    Task<T> CreateAsync(T entity);
    Task<T?> GetByIdAsync(int id);
    Task<List<T>> GetAll();
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task DeleteAsync(T entity);
    Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> TakeAsync<TKey>(int from, int to, Expression<Func<T, TKey>> selector);
}