using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.API.Infrastructure.EntityConfigurations
{
    public class CustomerBasketConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.OrderProducts).WithOne(x => x.Order).HasForeignKey(x => x.OrderId).IsRequired();
            builder.Property(x => x.CustomerEmail).IsRequired();
        }
    }
}
