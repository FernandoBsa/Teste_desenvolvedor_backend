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
    
    public async Task<Result<IEnumerable<ProductResponse>>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
    
        if (products == null || !products.Any())
            return Result.Failure<IEnumerable<ProductResponse>>(Error.NotFound("Product.NotFound", "No products found"));
    
        var productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
    
        return Result.Success(productResponses);
    }
    
    public async Task<Result<ProductResponse>> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
    
        if (product == null)
            return Result.Failure<ProductResponse>(Error.NotFound("Product.NotFound", "Product not found"));
    
        var productResponse = _mapper.Map<ProductResponse>(product);
    
        return Result.Success(productResponse);
    }
    
    public async Task<Result> UpdateProductAsync(UpdateProductRequest productRequest)
    {
        if (productRequest == null)
            return Result.Failure(Error.NullValue);

        var existingProduct = await _productRepository.GetByIdAsync(productRequest.Id);
        
        if (existingProduct == null)
            return Result.Failure(Error.NotFound("Product.NotFound", "Product not found"));
        
        existingProduct.UpdateProduct(productRequest.Name, productRequest.Description, productRequest.Price, productRequest.Stock);

        await _productRepository.SaveChangesAsync();
        
        return Result.Success();
    }
    
    public async Task<Result> DeleteProductAsync(Guid id)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        
        if (existingProduct == null)
            return Result.Failure(Error.NotFound("Product.NotFound", "Product not found"));
        
        await _productRepository.DeleteAsync(existingProduct);
        await _productRepository.SaveChangesAsync();
        
        return Result.Success();
    }
}