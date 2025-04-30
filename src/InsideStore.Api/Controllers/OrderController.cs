using InsideStore.Api.ResultExtensions;
using InsideStore.Application.Interfaces;
using InsideStore.Domain.DTO.Request.Order;
using InsideStore.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace InsideStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderServices _orderServices;

    public OrderController(IOrderServices orderServices)
    {
        _orderServices = orderServices;
    }
 
    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] PaginationFilterRequest paginationFilter)
    {
        var result = await _orderServices.GetAllOrdemAndItemsAsync(paginationFilter.Status, paginationFilter.Page, paginationFilter.PageSize);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
    
    [HttpPost("open")]
    public async Task<IResult> CreateOrder()
    {
        var result = await _orderServices.CreateOrderAsync();
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.Created();
    }
    
    [HttpPut("close/{id:guid}")]
    public async Task<IResult> CloseOrder(Guid id)
    {
        var result = await _orderServices.CloseOrderAsync(id);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.NoContent();
    }
    
    [HttpPost("{orderId}/items")]
    public async Task<IResult> AddItem(Guid orderId, [FromBody] AddItemRequest request)
    {
        var result = await _orderServices.AddItemToOrderAsync(orderId, request.ProductId, request.Quantity);
        
        if (result.IsFailure)
            return result.ToProblem();
        
        return Results.Ok();
    }

    [HttpPut("{orderId}/items/{productId}")]
    public async Task<IResult> RemoveItem(Guid orderId, Guid productId, [FromQuery] int quantity = 1)
    {
        var result = await _orderServices.RemoveItemFromOrderAsync(orderId, productId, quantity);
        
        if (result.IsFailure)
            return result.ToProblem();
        
        return Results.NoContent();
    }
}