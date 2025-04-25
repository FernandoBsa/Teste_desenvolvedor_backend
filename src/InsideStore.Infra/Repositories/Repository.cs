using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace InsideStore.Infra.Repositories;

public class Repository<T> : IRepository<T> where T : EntityBase
{
    private readonly InsideStoreContext _context;
    private readonly DbSet<T> _dbSet; 

    public Repository(InsideStoreContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<IEnumerable<T>?> GetAllAsync(int skip = 0, int take = 25, CancellationToken cancellationToken = default) =>
        await _dbSet.AsNoTracking().Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => 
        await _dbSet.FindAsync(id, cancellationToken);

    public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return entity;
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync() > 0;
    }
}