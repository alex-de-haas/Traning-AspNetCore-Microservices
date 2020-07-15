using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderUpdateCommandHandler : IRequestHandler<OrderUpdateCommand>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public OrderUpdateCommandHandler(IBasketDbContext context, IUserContextManager userContextManager)
        {
            _context = context;
            _userContextManager = userContextManager;
        }

        public async Task<Unit> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
        {
            var customerEmail = _userContextManager.GetCurrentUserEmail();
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId && x.CustomerEmail == customerEmail, cancellationToken);
            order.SetStatus(request.Status);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
