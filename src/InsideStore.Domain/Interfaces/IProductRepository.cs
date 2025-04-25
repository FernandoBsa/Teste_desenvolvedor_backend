using InsideStore.Domain.Entities;

namespace InsideStore.Domain.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<bool> ExistsByNameAsync(string name);
}