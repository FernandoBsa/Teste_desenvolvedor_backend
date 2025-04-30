using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Results;
using InsideStore.Application.Services;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.ProductServicesTest;

public class CreateProductTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductServices _productServices;

    public CreateProductTest()
    {
        _productServices = new ProductServices(
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldReturnFailure_WhenRequestIsNull()
    {
        // Act
        var result = await _productServices.CreateProductAsync(null);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Error.NullValue);
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldReturnFailure_WhenProductAlreadyExists()
    {
        // Arrange
        var request = new CreateProductRequest("Produto A", "Teste", 10, 5);

        _productRepositoryMock
            .Setup(r => r.ExistsByNameAsync(request.Name))
            .ReturnsAsync(true);

        // Act
        var result = await _productServices.CreateProductAsync(request);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.AlreadyExists");
        result.Error.Description.Should().Be("Product already exists");
    }
    
    [Fact]
    public async Task CreateProductAsync_ShouldCreateProductSuccessfully_WhenDataIsValid()
    {
        // Arrange
        var request = new CreateProductRequest("Produto A", "Teste", 10, 5);
        
        var product = new Product("Produto A", "Descrição", 10, 5);

        _productRepositoryMock
            .Setup(r => r.ExistsByNameAsync(request.Name))
            .ReturnsAsync(false);

        _productRepositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _productRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _productServices.CreateProductAsync(request);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _productRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        _productRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}