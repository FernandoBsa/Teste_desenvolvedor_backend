using InsideStore.Application.Interfaces;
using InsideStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InsideStore.Application.ServiceCollection;

public static class ApplicationServiceCollection
{
    public static IServiceCollection AddApplicationServicesCollection(this IServiceCollection services)
    {
        services.AddScoped<IOrderServices, OrderServices>();
        services.AddScoped<IProductServices, ProductServices>();
        
        return services;
    }
}