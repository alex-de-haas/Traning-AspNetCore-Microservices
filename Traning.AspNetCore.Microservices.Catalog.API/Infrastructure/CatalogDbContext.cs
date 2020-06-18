using Microsoft.EntityFrameworkCore;
using Traning.AspNetCore.Microservices.Catalog.API.Infrastructure.EntityConfigurations;
using Traning.AspNetCore.Microservices.Catalog.Application;
using Traning.AspNetCore.Microservices.Catalog.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Catalog.API.Infrastructure
{
    public class CatalogDbContext : DbContext, ICatalogDbContext
    {
        public DbSet<Product> Products { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }
    }
}
