using InsideStore.Api.ResultExtensions;
using InsideStore.Application.Interfaces;
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
    
    [HttpPost]
    public async Task<IResult> CreateOrder()
    {
        var result = await _orderServices.CreateOrderAsync();
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.Created();
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IResult> CloseOrder(Guid id)
    {
        var result = await _orderServices.CloseOrderAsync(id);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.NoContent();
    }
}