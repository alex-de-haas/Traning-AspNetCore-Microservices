using Ascetic.Microservices.Application.Managers;
using FluentValidation;
using Traning.AspNetCore.Microservices.Basket.Application.Extensions;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderUpdateCommandValidator : AbstractValidator<OrderUpdateCommand>
    {
        public OrderUpdateCommandValidator(IBasketDbContext context, IUserContextManager userContextManager)
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.OrderId).OrderExists(context, userContextManager);
        }
    }
}
