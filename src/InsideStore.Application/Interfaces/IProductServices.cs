using InsideStore.Application.Results;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.DTO.Response.Product;

namespace InsideStore.Application.Interfaces;

public interface IProductServices
{
    Task<Result> CreateProductAsync(CreateProductRequest productRequest);
    Task<Result<IEnumerable<AllProductResponse>>> GetAllProductsAsync();
}