using System.Linq.Expressions;
using Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Dal;

public abstract class BaseRepo<T> : IRepo<T> where T : Entity
{
    protected DbSet<T> Table { get; }
    protected MusicDbContext Db { get; }
    
    protected BaseRepo(MusicDbContext dbContext)
    {
        Db = dbContext;
        Table = Db.Set<T>();
    }
    
    public async Task<T> CreateAsync(T entity)
    {
        Table.Add(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await Table.FindAsync(id);
    }

    public async Task<List<T>> GetAll()
    {
        return await Table.ToListAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        Table.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
            await DeleteAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        Table.Remove(entity);
        await SaveChangesAsync();
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.FirstOrDefaultAsync(predicate);
    }

    public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> predicate)
    {
        return await Table.Where(predicate).ToListAsync();
    }

    public async Task<List<T>> TakeAsync<TKey>(int from, int to, Expression<Func<T, TKey>> selector)
    {
        return await Table.OrderBy(selector)
            .Take(new Range(from, to))
            .ToListAsync();
    }

    protected async Task SaveChangesAsync()
    {
        await Db.SaveChangesAsync();
    }
}