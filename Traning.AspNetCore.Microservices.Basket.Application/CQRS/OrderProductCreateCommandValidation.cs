using FluentValidation;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductCreateCommandValidation : AbstractValidator<OrderProductCreateCommand>
    {
        public OrderProductCreateCommandValidation()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
