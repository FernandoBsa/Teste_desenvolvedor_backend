namespace InsideStore.Domain.DTO.Request.Product;

public record CreateProductRequest(string Name, string Description, decimal Price, int Stock);