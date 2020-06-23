using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductCreateCommandValidation : AbstractValidator<OrderProductCreateCommand>
    {
        private readonly IProductsClient _productsClient;

        public OrderProductCreateCommandValidation(IProductsClient productsClient)
        {
            _productsClient = productsClient;

            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductId).MustAsync(ProductExistsAsync).WithMessage(x => $"Product with id = {x.ProductId} not exists.");
        }

        public async Task<bool> ProductExistsAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _productsClient.GetProductAsync(productId, cancellationToken);
            return product != null;
        }
    }
}
