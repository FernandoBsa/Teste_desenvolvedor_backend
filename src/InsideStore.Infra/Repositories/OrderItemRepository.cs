using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;

namespace InsideStore.Infra.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(InsideStoreContext context) : base(context)
    {
    }
}