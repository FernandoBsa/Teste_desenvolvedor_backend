using InsideStore.Application.Results;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.DTO.Response.Product;

namespace InsideStore.Application.Interfaces;

public interface IProductServices
{
    Task<Result> CreateProductAsync(CreateProductRequest productRequest);
    Task<Result<IEnumerable<ProductResponse>>> GetAllProductsAsync();
    Task<Result<ProductResponse>> GetByIdAsync(Guid id);
    Task<Result> UpdateProductAsync(UpdateProductRequest productRequest);
    Task<Result> DeleteProductAsync(Guid id);
}