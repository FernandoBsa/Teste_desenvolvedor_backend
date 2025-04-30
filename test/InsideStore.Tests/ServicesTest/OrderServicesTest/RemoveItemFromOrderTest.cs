using AutoMapper;
using FluentAssertions;
using InsideStore.Application.Services;
using InsideStore.Domain.Entities;
using InsideStore.Domain.Interfaces;
using Moq;

namespace InsideStore.Tests.ServicesTest.OrderServicesTest;

public class RemoveItemFromOrderTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
    private readonly Mock<IProductRepository> _productRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly OrderServices _orderServices;

    public RemoveItemFromOrderTest()
    {
        _orderServices = new OrderServices(
            _orderRepositoryMock.Object,
            _productRepositoryMock.Object,
            _mapperMock.Object
        );
    }
    
    [Fact]
    public async Task RemoveItemFromOrderAsync_ShouldReturnNotFound_WhenOrderDoesNotExist()
    {
        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order)null!);

        var result = await _orderServices.RemoveItemFromOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 1);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.NotFound");
    }

    [Fact]
    public async Task RemoveItemFromOrderAsync_ShouldReturnError_WhenOrderIsClosed()
    {
        var order = new Order();
        order.Close(); // Marca o pedido como fechado

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.RemoveItemFromOrderAsync(Guid.NewGuid(), Guid.NewGuid(), 1);

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.Closed");
    }

    [Fact]
    public async Task RemoveItemFromOrderAsync_ShouldReturnBadRequest_WhenRemovingMoreThanAvailableQuantity()
    {
        var order = new Order();
        var product = new Product("Produto A", "Descrição", 100, 10);
        order.AddItem(product, 5);  // Adiciona 5 unidades do produto

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        var result = await _orderServices.RemoveItemFromOrderAsync(Guid.NewGuid(), product.Id, 10);  // Tenta remover 10, o que é maior que a quantidade

        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("Order.InvalidQuantity");
    }

    [Fact]
    public async Task RemoveItemFromOrderAsync_ShouldRemoveItemAndReturnSuccess_WhenDataIsValid()
    {
        var product = new Product("Produto A", "Descrição", 100, 10);
        var order = new Order();
        order.AddItem(product, 5); // Adiciona 5 unidades do produto

        _orderRepositoryMock
            .Setup(r => r.GetOrderAndItemsAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);

        _orderRepositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _orderServices.RemoveItemFromOrderAsync(Guid.NewGuid(), product.Id, 2); // Remove 2 unidades

        result.IsSuccess.Should().BeTrue();
        order.Items.Should().ContainSingle(i => i.ProductId == product.Id && i.Quantity == 3); // Deve sobrar 3 unidades
        _orderRepositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}