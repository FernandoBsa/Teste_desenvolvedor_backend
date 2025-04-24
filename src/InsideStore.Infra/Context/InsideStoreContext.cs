using InsideStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace InsideStore.Infra.Context
{
    public class InsideStoreContext : DbContext
    {
        public InsideStoreContext(DbContextOptions<InsideStoreContext> options) : base(options)
        {
        }
      
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InsideStoreContext).Assembly);
        }
    }
}
