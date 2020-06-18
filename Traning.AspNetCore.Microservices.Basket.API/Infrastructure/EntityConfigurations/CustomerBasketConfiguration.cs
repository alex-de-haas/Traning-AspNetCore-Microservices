using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.API.Infrastructure.EntityConfigurations
{
    public class CustomerBasketConfiguration : IEntityTypeConfiguration<CustomerBasket>
    {
        public void Configure(EntityTypeBuilder<CustomerBasket> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Products).WithOne().HasForeignKey(x => x.BasketId).IsRequired();
            builder.Property(x => x.CustomerEmail).IsRequired();
        }
    }
}
