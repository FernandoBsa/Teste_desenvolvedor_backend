using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace InsideStore.Infra.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    
    private readonly InsideStoreContext _context;
    public ProductRepository(InsideStoreContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Products.AnyAsync(p => p.Name.ToLower() == name.ToLower());
    }
}