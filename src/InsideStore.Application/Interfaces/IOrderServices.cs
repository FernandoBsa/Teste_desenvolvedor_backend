using InsideStore.Application.Results;

namespace InsideStore.Application.Interfaces;

public interface IOrderServices
{
    Task<Result> CreateOrderAsync();
    Task<Result> CloseOrderAsync(Guid id);
}