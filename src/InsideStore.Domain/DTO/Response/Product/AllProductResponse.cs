namespace InsideStore.Domain.DTO.Response.Product;

public record AllProductResponse(string Name, string Description, decimal Price, int Stock);