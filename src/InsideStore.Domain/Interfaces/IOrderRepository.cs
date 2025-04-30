using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;

namespace InsideStore.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetOrderAndItemsAsync(Guid orderId);
    Task<IEnumerable<Order>> GetAllOrdersAndItemsAsync(OrderStatus? status = null, int skip = 0, int take = 25);
}