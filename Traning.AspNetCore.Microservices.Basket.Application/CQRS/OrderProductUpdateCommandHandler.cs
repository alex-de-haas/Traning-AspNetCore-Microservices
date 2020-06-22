using Ascetic.Microservices.Application.Exceptions;
using Ascetic.Microservices.Application.Managers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductUpdateCommandHandler : IRequestHandler<OrderProductUpdateCommand>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public OrderProductUpdateCommandHandler(IBasketDbContext context, IUserContextManager userContextManager)
        {
            _context = context;
            _userContextManager = userContextManager;
        }

        public async Task<Unit> Handle(OrderProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContextManager.GetCurrentUser();
            var customerEmail = currentUser.FindFirst("preferred_username").Value;
            var order = await _context.Orders.Include(x => x.OrderProducts).FirstOrDefaultAsync(x => x.Id == request.OrderId && x.CustomerEmail == customerEmail, cancellationToken);
            if (order == null)
            {
                throw new EntityNotFoundException($"Order with id = '{request.OrderId}' for customer with email = '{customerEmail}' not found.");
            }
            var product = order.OrderProducts.FirstOrDefault(x => x.ProductId == request.ProductId);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id = '{request.ProductId}' in order with id = '{request.OrderId}' not found.");
            }
            product.Update(request.Quantity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
