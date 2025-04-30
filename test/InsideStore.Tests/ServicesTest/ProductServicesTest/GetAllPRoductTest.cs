using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.DTO.Response.Product;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.ProductServicesTest;

public class GetAllPRoductTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductServices _productServices;

    public GetAllPRoductTest()
    {
        _productServices = new ProductServices(
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnSuccess_WhenProductsExist()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product("Produto A", "Desc A", 10, 5),
            new Product("Produto B", "Desc B", 20, 10)
        };

        var responses = new List<ProductResponse>
        {
            new ProductResponse(Guid.NewGuid(),"Produto A", "Desc A", 10, 5),
            new ProductResponse(Guid.NewGuid(),"Produto B", "Desc B", 20, 10)
        };

        _productRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);
        
        _mapperMock.Setup(m => m.Map<IEnumerable<ProductResponse>>(products)).Returns(responses);

        // Act
        var result = await _productServices.GetAllProductsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(r => r.Name == "Produto A");
        result.Value.Should().Contain(r => r.Name == "Produto B");
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnNotFound_WhenProductsIsNull()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Product>)null);

        // Act
        var result = await _productServices.GetAllProductsAsync();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.NotFound");
    }

    [Fact]
    public async Task GetAllProductsAsync_ShouldReturnNotFound_WhenProductsIsEmpty()
    {
        // Arrange
        _productRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productServices.GetAllProductsAsync();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.NotFound");
    }
}