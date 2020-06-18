using FluentValidation;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class BasketUpdateCommandValidation : AbstractValidator<BasketUpdateCommand>
    {
        public BasketUpdateCommandValidation()
        {
            RuleFor(x => x.ProductIds).NotNull();
        }
    }
}
