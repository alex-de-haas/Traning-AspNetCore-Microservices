using FluentValidation;
using Traning.AspNetCore.Microservices.Catalog.Application.Extensions;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductDeleteCommandValidation : AbstractValidator<ProductDeleteCommand>
    {
        public ProductDeleteCommandValidation(ICatalogDbContext context)
        {
            RuleFor(x => x.ProductId).ProductExists(context);
        }
    }
}
