using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace Traning.AspNetCore.Microservices.Basket.Application.Extensions
{
    public static class OrderValidationExtensions
    {
        public static IRuleBuilderOptions<T, Guid> OrderExists<T>(this IRuleBuilderInitial<T, Guid> builder, IBasketDbContext context, IUserContextManager userContextManager)
        {
            return builder.MustAsync(async (Guid orderId, CancellationToken cancellationToken) =>
            {
                var customerEmail = userContextManager.GetCurrentUserEmail();
                var order = await context.Orders.FirstOrDefaultAsync(x => x.Id == orderId && x.CustomerEmail == customerEmail, cancellationToken);
                return order != null;
            }).WithErrorCode("OrderNotExists");
        }
    }
}
