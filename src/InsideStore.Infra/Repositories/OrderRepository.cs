using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;

namespace InsideStore.Infra.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(InsideStoreContext context) : base(context)
    {
    }
}