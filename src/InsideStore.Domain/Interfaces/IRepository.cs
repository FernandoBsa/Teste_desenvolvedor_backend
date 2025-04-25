using InsideStore.Domain.Entities;

namespace InsideStore.Domain.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>?> GetAllAsync(int skip = 0, int take = 25, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}