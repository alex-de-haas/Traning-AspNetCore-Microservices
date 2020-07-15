using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Application.Extensions;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductUpdateCommandValidation : AbstractValidator<OrderProductUpdateCommand>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public OrderProductUpdateCommandValidation(IBasketDbContext context, IUserContextManager userContextManager, IProductsClient productsClient)
        {
            _context = context;
            _userContextManager = userContextManager;

            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.OrderId).OrderExists(context, userContextManager);
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.ProductId).ProductExists(productsClient);
            RuleFor(x => x.Quantity).NotEmpty();
            RuleFor(x => x).MustAsync(ProductInOrderAsync).WithErrorCode("ProductNotInOrder");
        }

        public async Task<bool> ProductInOrderAsync(OrderProductUpdateCommand command, CancellationToken cancellationToken)
        {
            var customerEmail = _userContextManager.GetCurrentUserEmail();
            var order = await _context.Orders.Include(x => x.OrderProducts).FirstOrDefaultAsync(x => x.Id == command.OrderId && x.CustomerEmail == customerEmail, cancellationToken);
            if (order == null)
            {
                return false;
            }
            var product = order.OrderProducts.FirstOrDefault(x => x.ProductId == command.ProductId);
            return product != null;
        }
    }
}
