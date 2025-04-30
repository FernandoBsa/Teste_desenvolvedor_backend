using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Enum;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.OrderServicesTest;

public class CloseOrderTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly OrderServices _orderServices;

    public CloseOrderTest()
    {
        _orderServices = new OrderServices(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order)null!);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.NotFound");
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldReturnError_WhenOrderIsAlreadyClosed()
    {
        var order = new Order();
        order.Close();

        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.AlreadyClosed");
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldReturnError_WhenOrderHasNoItems()
    {
        var order = new Order();

        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.Empty");
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldReturnError_WhenItemHasInvalidQuantity()
    {
        var order = new Order();

        var product = new Product("Produto A", "Descricao do Produto A", 100, 1);

        order.AddItem(product, 0);

        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.InvalidQuantity");
        result.Error.Description.Should().Contain("Produto A");
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldReturnError_WhenStockIsInsufficient()
    {
        var order = new Order();
        var product = new Product("Produto A", "Descricao do Produto A", 100, 1);

        order.AddItem(product, 5);

        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Product.InsufficientStock");
        result.Error.Description.Should().Contain("Produto A");
    }

    [Fact]
    public async Task CloseOrderAsync_ShouldCloseOrderSuccessfully_WhenOrderIsValid()
    {
        var product = new Product("Produto A", "Descricao do Produto A", 100, 10);
        
        var order = new Order();
        
        order.AddItem(product, 2);

        _orderRepositoryMock.Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        _orderRepositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var result = await _orderServices.CloseOrderAsync(Guid.NewGuid());

        result.IsSuccess.Should().BeTrue();
        order.Status.Should().Be(OrderStatus.Closed);
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}