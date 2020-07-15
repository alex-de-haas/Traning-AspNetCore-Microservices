using Ascetic.Microservices.Application.Managers;
using FluentValidation;
using Traning.AspNetCore.Microservices.Basket.Application.Extensions;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductDeleteCommandValidation : AbstractValidator<OrderProductDeleteCommand>
    {
        public OrderProductDeleteCommandValidation(IBasketDbContext context, IUserContextManager userContextManager, IProductsClient productsClient)
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.OrderId).OrderExists(context, userContextManager);
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductId).ProductExists(productsClient);
        }
    }
}
