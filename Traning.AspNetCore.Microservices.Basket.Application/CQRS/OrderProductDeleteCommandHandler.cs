using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductDeleteCommandHandler : IRequestHandler<OrderProductDeleteCommand>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public OrderProductDeleteCommandHandler(IBasketDbContext context, IUserContextManager userContextManager)
        {
            _context = context;
            _userContextManager = userContextManager;
        }

        public async Task<Unit> Handle(OrderProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var customerEmail = _userContextManager.GetCurrentUserEmail();
            var order = await _context.Orders.Include(x => x.OrderProducts).FirstOrDefaultAsync(x => x.Id == request.OrderId && x.CustomerEmail == customerEmail, cancellationToken);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
