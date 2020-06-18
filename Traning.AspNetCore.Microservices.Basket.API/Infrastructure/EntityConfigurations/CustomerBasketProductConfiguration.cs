using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.API.Infrastructure.EntityConfigurations
{
    public class CustomerBasketProductConfiguration : IEntityTypeConfiguration<CustomerBasketProduct>
    {
        public void Configure(EntityTypeBuilder<CustomerBasketProduct> builder)
        {
            builder.HasKey(x => new { x.BasketId, x.ProductId });
        }
    }
}
