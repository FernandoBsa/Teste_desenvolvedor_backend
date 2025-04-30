using InsideStore.Application.Results;
using InsideStore.Domain.DTO.Request.Order;
using InsideStore.Domain.DTO.Response.Order;
using InsideStore.Domain.Enum;

namespace InsideStore.Application.Interfaces;

public interface IOrderServices
{
    Task<Result> CreateOrderAsync();
    Task<Result<IEnumerable<OrderResponse>>> GetAllOrdemAndItemsAsync(OrderStatus? status = null, int skip = 0, int take = 25);
    Task<Result> CloseOrderAsync(Guid id);
    Task<Result> AddItemToOrderAsync(Guid orderId, Guid productId, int quantity);
    Task<Result> RemoveItemFromOrderAsync(Guid orderId, Guid productId, int quantity);
    
}