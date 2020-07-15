using System;
using System.Threading;
using FluentValidation;

namespace Traning.AspNetCore.Microservices.Catalog.Application.Extensions
{
    public static class ProductValidationExtensions
    {
        public static IRuleBuilderOptions<T, Guid> ProductExists<T>(this IRuleBuilderInitial<T, Guid> builder, ICatalogDbContext context)
        {
            return builder.MustAsync(async (Guid productId, CancellationToken cancellationToken) =>
            {
                var product = await context.Products.FindAsync(new object[] { productId }, cancellationToken);
                return product != null;
            }).WithErrorCode("ProductNotExists");
        }
    }
}
