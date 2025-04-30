namespace InsideStore.Domain.DTO.Response.Order;

public record OrderItemResponse(
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal SubTotal);