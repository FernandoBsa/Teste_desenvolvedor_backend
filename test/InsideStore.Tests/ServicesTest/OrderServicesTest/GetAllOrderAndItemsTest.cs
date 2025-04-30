using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.DTO.Response.Order;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.OrderServicesTest;

public class GetAllOrderAndItemsTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly OrderServices _orderServices;

    public GetAllOrderAndItemsTest()
    {
        _orderServices = new OrderServices(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAllOrdemAndItemsAsync_ShouldReturnSuccess_WhenOrdersExist()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order(),
            new Order()
        };

        var mappedOrders = new List<OrderResponse>
        {
            new OrderResponse(Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, OrderStatus.Closed.ToString(), 200, new List<OrderItemResponse>()),
            new OrderResponse(Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, OrderStatus.Closed.ToString(), 250, new List<OrderItemResponse>())
        };

        _orderRepositoryMock
            .Setup(repo => repo.GetAllOrdersAndItemsAsync(null, 0, 25))
            .ReturnsAsync(orders);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<OrderResponse>>(orders))
            .Returns(mappedOrders);

        // Act
        var result = await _orderServices.GetAllOrdemAndItemsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(mappedOrders);
    }
    
    [Fact]
    public async Task GetAllOrderAndItemsAsync_ShouldReturnFailure_WhenNoOrdersFound()
    {
        // Arrange
        _orderRepositoryMock
            .Setup(repo => repo.GetAllOrdersAndItemsAsync(null, 0, 25))
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _orderServices.GetAllOrdemAndItemsAsync();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be("Order.NotFound");
    }
}