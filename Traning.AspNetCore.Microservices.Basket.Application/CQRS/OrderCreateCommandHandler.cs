using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderCreateCommandHandler : IRequestHandler<OrderCreateCommand, Guid>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public OrderCreateCommandHandler(IBasketDbContext context, IUserContextManager userContextManager)
        {
            _context = context;
            _userContextManager = userContextManager;
        }

        public async Task<Guid> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
        {
            var customerEmail = _userContextManager.GetCurrentUserEmail();
            var order = new Order(customerEmail);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
