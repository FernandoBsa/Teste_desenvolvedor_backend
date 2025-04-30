using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;
using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace InsideStore.Infra.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly InsideStoreContext _context;
    public OrderRepository(InsideStoreContext context) : base(context)
    {
        _context = context;
    }
    public async Task<Order> GetOrderAndItemsAsync(Guid orderId)
    {
        return await _context.Orders.Where(o => o.Id == orderId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync();

    }
    
    public async Task<IEnumerable<Order>> GetAllOrdersAndItemsAsync(OrderStatus? status = null, int skip = 0, int take = 25)
    {
        return await _context.Orders
            .Where(o => !status.HasValue || o.Status == status.Value)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}