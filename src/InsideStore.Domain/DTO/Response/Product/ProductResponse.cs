namespace InsideStore.Domain.DTO.Response.Product;

public record ProductResponse(Guid Id, string Name, string Description, decimal Price, int Stock);