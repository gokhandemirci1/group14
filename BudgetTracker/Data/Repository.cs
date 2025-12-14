using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using BudgetTracker.Data;

namespace BudgetTracker.Data;

/// <summary>
/// Generic Repository Pattern Implementation.
/// SOLID - Single Responsibility: Sadece veri erişim işlemlerinden sorumlu.
/// Tüm entity'ler için ortak CRUD operasyonlarını sağlar.
/// </summary>
/// <typeparam name="T">Entity tipi</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public virtual async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public virtual void Update(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public virtual void Delete(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _dbSet.Remove(entity);
        _context.SaveChanges();
    }

    public virtual async Task<bool> DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        Delete(entity);
        return true;
    }

    public virtual async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        if (predicate == null)
            return await _dbSet.CountAsync();

        return await _dbSet.CountAsync(predicate);
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }
}




