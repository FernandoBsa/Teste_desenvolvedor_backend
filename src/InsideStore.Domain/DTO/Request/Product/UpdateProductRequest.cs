namespace InsideStore.Domain.DTO.Request.Product;

public record UpdateProductRequest(Guid Id, string? Name, string? Description, decimal? Price, int? Stock);
