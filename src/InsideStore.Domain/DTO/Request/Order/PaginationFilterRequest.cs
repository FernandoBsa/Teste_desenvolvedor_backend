using InsideStore.Domain.Enum;

namespace InsideStore.Domain.DTO.Request.Order;

public record PaginationFilterRequest(int Page = 0, int PageSize = 25, OrderStatus? Status = null);