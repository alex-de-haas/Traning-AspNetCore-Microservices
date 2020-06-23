using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductUpdateCommandValidator : AbstractValidator<ProductUpdateCommand>
    {
        private readonly ICatalogDbContext _context;

        public ProductUpdateCommandValidator(ICatalogDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ProductId).MustAsync(ProductExistsAsync).WithMessage(x => $"Product with id = '{x.ProductId}' not exists.");
        }

        public async Task<bool> ProductExistsAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { productId }, cancellationToken);
            return product != null;
        }
    }
}
