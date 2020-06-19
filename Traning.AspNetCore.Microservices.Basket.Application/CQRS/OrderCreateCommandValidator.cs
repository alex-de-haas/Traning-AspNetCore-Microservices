using FluentValidation;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderCreateCommandValidator : AbstractValidator<OrderCreateCommand>
    {
        public OrderCreateCommandValidator()
        {
        }
    }
}
