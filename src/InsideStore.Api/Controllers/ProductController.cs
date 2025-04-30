using InsideStore.Api.ResultExtensions;
using InsideStore.Application.Interfaces;
using InsideStore.Domain.DTO.Request.Product;
using Microsoft.AspNetCore.Mvc;

namespace InsideStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductServices _productService;

    public ProductController(IProductServices productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public async Task<IResult> CreateProduct([FromBody] CreateProductRequest product)
    {
        var result = await _productService.CreateProductAsync(product);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.Created();
    }
    
    [HttpGet]
    public async Task<IResult> GetAllProducts()
    {
        var result = await _productService.GetAllProductsAsync();
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.Ok(result.Value);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IResult> GetProductById(Guid id)
    {
        var result = await _productService.GetByIdAsync(id);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.Ok(result.Value);
    }
    
    [HttpPut]
    public async Task<IResult> UpdateProduct([FromBody] UpdateProductRequest product)
    {
        var result = await _productService.UpdateProductAsync(product);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IResult> DeleteProduct(Guid id)
    {
        var result = await _productService.DeleteProductAsync(id);
        
        if (result.IsFailure)
            return result.ToProblem();

        return Results.NoContent();
    }
}