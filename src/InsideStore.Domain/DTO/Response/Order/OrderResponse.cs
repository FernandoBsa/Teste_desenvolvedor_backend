namespace InsideStore.Domain.DTO.Response.Order;

public record OrderResponse(
    Guid Id,
    DateTime CreatedAt,
    DateTime? ClosedAt,
    string Status,
    decimal Total,
    IEnumerable<OrderItemResponse> Items);
