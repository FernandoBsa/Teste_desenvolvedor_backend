using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.OrderServicesTest;

public class AddItemToOrderTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly OrderServices _orderServices;

    public AddItemToOrderTest()
    {
        _orderServices = new OrderServices(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task AddItemToOrderAsync_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order)null!);

        var result = await _orderServices.AddItemToOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 2);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.NotFound");
    }

    [Fact]
    public async Task AddItemToOrderAsync_ShouldReturnError_WhenOrderIsClosed()
    {
        var order = new Order();
        order.Close();

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.AddItemToOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 2);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.Closed");
    }

    [Fact]
    public async Task AddItemToOrderAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        var order = new Order();

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null!);

        var result = await _orderServices.AddItemToOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 2);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.NotFound");
    }

    [Fact]
    public async Task AddItemToOrderAsync_ShouldAddItemAndReturnSuccess_WhenDataIsValid()
    {
        var product = new Product("Produto A", "Descrição", 100, 10);
        var order = new Order();

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        _productRepositoryMock
            .Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _orderRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _orderServices.AddItemToOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 2);

        result.IsSuccess.Should().BeTrue();
        order.Items.Should().ContainSingle(i => i.Quantity == 2 && i.Product != null && i.Product.Id == product.Id);
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}