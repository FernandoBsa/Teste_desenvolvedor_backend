using InsideStore.Application.AutoMapper;
using InsideStore.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace InsideStore.Api.Configuration;

public static class ApiConfig
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<InsideStoreContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddAutoMapper(typeof(AutoMapperProfile));
        
        services.AddDependencyInjection();
        
        return services;
    }
}