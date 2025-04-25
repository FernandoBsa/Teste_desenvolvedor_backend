using InsideStore.Domain.Interfaces;
using InsideStore.Infra.Context;
using InsideStore.Infra.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InsideStore.Infra.ServiceCollection;

public static class InfraServiceCollection 
{
    public static IServiceCollection AddInfraServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<InsideStoreContext>();
        
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        
        return services;
    }
}