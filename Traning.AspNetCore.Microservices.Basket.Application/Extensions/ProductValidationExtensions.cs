using FluentValidation;
using System;
using System.Threading;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.Extensions
{
    public static class ProductValidationExtensions
    {
        public static IRuleBuilderOptions<T, Guid> ProductExists<T>(this IRuleBuilderInitial<T, Guid> builder, IProductsClient client)
        {
            return builder.MustAsync(async (Guid productId, CancellationToken cancellationToken) =>
            {
                var product = await client.GetProductAsync(productId, cancellationToken);
                return product != null;
            }).WithErrorCode("ProductNotExists");
        }
    }
}
