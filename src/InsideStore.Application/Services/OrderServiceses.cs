using InsideStore.Application.Interfaces;
using InsideStore.Application.Results;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;
using InsideStore.Domain.Interfaces;

namespace InsideStore.Application.Services;

public class OrderServiceses : IOrderServices
{
    private readonly IOrderRepository _orderRepository;

    public OrderServiceses(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> CreateOrderAsync()
    {
        var order = new Order();
        
        await _orderRepository.CreateAsync(order);
        await _orderRepository.SaveChangesAsync();
        
        return Result.Success();
    }

    public async Task<Result> CloseOrderAsync(Guid id)
    {
        var order = await _orderRepository.GetByIdAsync(id);
    
        if (order is null)
            return Result.Failure(Error.NotFound("Order.NotFound", "Pedido não encontrado"));
        
        if (order.Status == OrderStatus.Closed)
            return Result.Failure(Error.BadRequest("Order.AlreadyClosed", "Pedido já está fechado"));
        
        if (!order.Items.Any())
            return Result.Failure(Error.BadRequest("Order.Empty", "Não é possível fechar um pedido sem produtos"));
    
        order.Close();
    
        await _orderRepository.UpdateAsync(order);
        await _orderRepository.SaveChangesAsync();
    
        return Result.Success();
    }
}