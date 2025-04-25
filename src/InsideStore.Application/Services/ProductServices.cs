using AutoMapper;
using InsideStore.Application.Interfaces;
using InsideStore.Application.Results;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.DTO.Response.Product;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;

namespace InsideStore.Application.Services;

public class ProductServices : IProductServices
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductServices(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    
    public async Task<Result> CreateProductAsync(CreateProductRequest productRequest)
    {
        if (productRequest == null)
            return Result.Failure(Error.NullValue);

        var existingProduct = await _productRepository.ExistsByNameAsync(productRequest.Name);
        
        if (existingProduct)
            return Result.Failure(Error.Conflict("Product.AlreadyExists", "Product already exists"));
        
        Product newProduct = new Product(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.Stock);

        await _productRepository.CreateAsync(newProduct);
        await _productRepository.SaveChangesAsync();
        
        return Result.Success();
    }
    
    public async Task<Result<IEnumerable<AllProductResponse>>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
    
        if (products == null || !products.Any())
            return Result.Failure<IEnumerable<AllProductResponse>>(Error.NotFound("Product.NotFound", "No products found"));
    
        var productResponses = _mapper.Map<IEnumerable<AllProductResponse>>(products);
    
        return Result.Success(productResponses);
    }
}