using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace InsideStore.Infra.Context
{
    public class InsideStoreContextFactory : IDesignTimeDbContextFactory<InsideStoreContext>
    {
        public InsideStoreContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../InsideStore.Api");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<InsideStoreContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new InsideStoreContext(optionsBuilder.Options);
        }
    }
}
