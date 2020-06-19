using FluentValidation;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductUpdateCommandValidation : AbstractValidator<OrderProductUpdateCommand>
    {
        public OrderProductUpdateCommandValidation()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
