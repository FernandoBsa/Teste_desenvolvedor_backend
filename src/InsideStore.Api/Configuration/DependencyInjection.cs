using InsideStore.Application.ServiceCollection;
using InsideStore.Infra.ServiceCollection;

namespace InsideStore.Api.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddInfraServiceCollection();
        services.AddApplicationServicesCollection();
        
        return services;
    }
}