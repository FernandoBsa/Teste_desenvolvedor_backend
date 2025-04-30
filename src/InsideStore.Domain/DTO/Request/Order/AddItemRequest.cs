namespace InsideStore.Domain.DTO.Request.Order;

public record AddItemRequest(Guid ProductId, int Quantity);
