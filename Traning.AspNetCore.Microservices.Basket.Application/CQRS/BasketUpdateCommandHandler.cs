using Ascetic.Microservices.Application.Managers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class BasketUpdateCommandHandler : IRequestHandler<BasketUpdateCommand>
    {
        private readonly IBasketDbContext _context;
        private readonly IUserContextManager _userContextManager;

        public BasketUpdateCommandHandler(IBasketDbContext context, IUserContextManager userContextManager)
        {
            _context = context;
            _userContextManager = userContextManager;
        }

        public async Task<Unit> Handle(BasketUpdateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContextManager.GetCurrentUser();
            var customerEmail = currentUser.FindFirst("preferred_username").Value;
            var basket = await _context.CustomerBaskets.Include(x => x.Products).FirstOrDefaultAsync(x => x.CustomerEmail == customerEmail, cancellationToken);
            if (basket == null)
            {
                basket = new CustomerBasket
                {
                    CustomerEmail = customerEmail,
                    Products = new List<CustomerBasketProduct>()
                };
                _context.CustomerBaskets.Add(basket);
            }
            basket.Products.Clear();
            foreach (var productId in request.ProductIds)
            {
                basket.Products.Add(new CustomerBasketProduct
                {
                    BasketId = basket.Id,
                    ProductId = productId
                });
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
