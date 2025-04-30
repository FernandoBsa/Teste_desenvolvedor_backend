using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.OrderServicesTest;

public class CreateOrderTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly OrderServices _orderServices;

    public CreateOrderTest()
    {
        _orderServices = new OrderServices(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrderAndReturnSuccess()
    {
        // Arrange
        _orderRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order());

        _orderRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _orderServices.CreateOrderAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();

        _orderRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}