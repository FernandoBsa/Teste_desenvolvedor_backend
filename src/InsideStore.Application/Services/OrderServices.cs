using AutoMapper;
using InsideStore.Application.Interfaces;
using InsideStore.Application.Results;
using InsideStore.Domain.DTO.Response.Order;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;
using InsideStore.Domain.Interfaces;

namespace InsideStore.Application.Services;

public class OrderServices : IOrderServices
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public OrderServices(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<IEnumerable<OrderResponse>>> GetAllOrdemAndItemsAsync(OrderStatus? status = null, int skip = 0, int take = 25)
    {
        var orders = await _orderRepository.GetAllOrdersAndItemsAsync(status, skip, take);

        if (orders == null || !orders.Any())
            return Result.Failure<IEnumerable<OrderResponse>>(Error.NotFound("Order.NotFound", "Nenhum pedido encontrado"));

        var orderResponses = _mapper.Map<IEnumerable<OrderResponse>>(orders);
    
        return Result.Success(orderResponses);
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
        var order = await _orderRepository.GetOrderAndItemsAsync(id);

        if (order is null)
            return Result.Failure(Error.NotFound("Order.NotFound", "Pedido não encontrado"));

        if (order.Status == OrderStatus.Closed)
            return Result.Failure(Error.BadRequest("Order.AlreadyClosed", "Pedido já está fechado"));

        if (!order.Items.Any())
            return Result.Failure(Error.BadRequest("Order.Empty", "Não é possível fechar um pedido sem produtos"));
        
        foreach (var item in order.Items)
        {
            if (item.Quantity <= 0)
                return Result.Failure(Error.BadRequest("Product.InvalidQuantity", 
                    $"Quantidade inválida para o produto {item.Product.Name}"));

            if (item.Product.Stock < item.Quantity)
                return Result.Failure(Error.BadRequest("Product.InsufficientStock", 
                    $"Estoque insuficiente para o produto {item.Product.Name}"));
        }

        order.Close();
        
        await _orderRepository.SaveChangesAsync();
        //await _productRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> AddItemToOrderAsync(Guid orderId, Guid productId, int quantity)
    {
        var order = await _orderRepository.GetOrderAndItemsAsync(orderId);

        if (order == null)
            return Result.Failure(Error.NotFound("Order.NotFound", "Pedido não encontrado"));

        if (order.Status == OrderStatus.Closed)
            return Result.Failure(Error.BadRequest("Order.Closed", "Não é possível adicionar itens a um pedido fechado"));

        var product = await _productRepository.GetByIdAsync(productId);

        if (product == null)
            return Result.Failure(Error.NotFound("Product.NotFound", "Produto não encontrado"));

        order.AddItem(product, quantity);
        
        await _orderRepository.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> RemoveItemFromOrderAsync(Guid orderId, Guid productId, int quantity)
    {
        var order = await _orderRepository.GetOrderAndItemsAsync(orderId);
        if (order == null)
            return Result.Failure(Error.NotFound("Order.NotFound", "Pedido não encontrado"));

        if (order.Status == OrderStatus.Closed)
            return Result.Failure(Error.BadRequest("Order.Closed", "Não é possível remover itens de um pedido fechado"));
        
        var item = order.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            return Result.Failure(Error.NotFound("OrderItem.NotFound", "Item do pedido não encontrado"));

        if (item.Quantity < quantity)
            return Result.Failure(Error.BadRequest("Order.InvalidQuantity", "Quantidade a remover é maior do que a disponível no pedido"));
        
        order.RemoveItem(productId, quantity);
        
        await _orderRepository.SaveChangesAsync();

        return Result.Success();
    }
}