using Microsoft.EntityFrameworkCore;
using Traning.AspNetCore.Microservices.Basket.API.Infrastructure.EntityConfigurations;
using Traning.AspNetCore.Microservices.Basket.Application;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.API.Infrastructure
{
    public class BasketDbContext : DbContext, IBasketDbContext
    {
        public DbSet<Order> Orders { get; set; }

        public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerBasketConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerBasketProductConfiguration());
        }
    }
}
