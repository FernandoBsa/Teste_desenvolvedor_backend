using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Results;
using InsideStore.Application.Services;
using InsideStore.Domain.DTO.Request.Product;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.ProductServicesTest;

public class UpdateProductTest
{
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly ProductServices _productServices;

    public UpdateProductTest()
    {
        _productServices = new ProductServices(
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnSuccess_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateRequest = new UpdateProductRequest(productId, "Produto Atualizado", "Nova descrição", 200, 50);

        var existingProduct = new Product("Antigo Nome", "Descrição Antiga", 100, 10);

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(productId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProduct);

        _productRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _productServices.UpdateProductAsync(updateRequest);

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingProduct.Name.Should().Be(updateRequest.Name);
        existingProduct.Description.Should().Be(updateRequest.Description);
        existingProduct.Price.Should().Be(updateRequest.Price);
        existingProduct.Stock.Should().Be(updateRequest.Stock);

        _productRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var updateRequest = new UpdateProductRequest(Guid.NewGuid(), "Produto Atualizado", "Nova descrição", 200, 50);

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(updateRequest.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null!);

        // Act
        var result = await _productServices.UpdateProductAsync(updateRequest);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.NotFound");

        _productRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
    
    [Fact]
    public async Task UpdateProductAsync_ShouldReturnFailure_WhenRequestIsNull()
    {
        // Act
        var result = await _productServices.UpdateProductAsync(null!);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(Error.NullValue);

        _productRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}