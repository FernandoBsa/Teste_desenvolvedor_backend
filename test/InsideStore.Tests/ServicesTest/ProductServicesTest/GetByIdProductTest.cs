using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.DTO.Response.Product;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.ProductServicesTest;

public class GetByIdProductTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductServices _productServices;

    public GetByIdProductTest()
    {
        _productServices = new ProductServices(
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnSuccess_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product("Produto A", "Descrição", 100, 10);
        var expectedResponse =
            new ProductResponse(product.Id, product.Name, product.Description, product.Price, product.Stock);

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _mapperMock
            .Setup(m => m.Map<ProductResponse>(product))
            .Returns(expectedResponse);

        // Act
        var result = await _productServices.GetByIdAsync(productId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedResponse);
        _productRepositoryMock.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.Map<ProductResponse>(product), Times.Once);
    }
    
    [Fact]
    public async Task GetByIdAsync_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null!);

        // Act
        var result = await _productServices.GetByIdAsync(productId);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Product.NotFound");
        _productRepositoryMock.Verify(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()), Times.Once);
    }
}