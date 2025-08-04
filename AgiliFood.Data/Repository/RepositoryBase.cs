using AgiliFood.Business.Interfaces;
using AgiliFood.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


public class RepositoryBase<T>(ApplicationDbContext context) : IRepositoryBase<T> where T : class
{
    protected readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().AsNoTracking()
                                      .ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }
    public T Update(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public T Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        return entity;
    }
}