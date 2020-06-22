using Ascetic.Microservices.Application.Exceptions;
using Ascetic.Microservices.Application.Managers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderViewQueryHandler : IRequestHandler<OrderViewQuery, OrderViewDto>
    {
        private readonly IBasketDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextManager _userContextManager;
        private readonly IProductsClient _productsClient;

        public OrderViewQueryHandler(IBasketDbContext context, IMapper mapper, IUserContextManager userContextManager, IProductsClient productsClient)
        {
            _context = context;
            _mapper = mapper;
            _userContextManager = userContextManager;
            _productsClient = productsClient;
        }

        public async Task<OrderViewDto> Handle(OrderViewQuery request, CancellationToken cancellationToken)
        {
            var currentUser = _userContextManager.GetCurrentUser();
            var customerEmail = currentUser.FindFirst("preferred_username").Value;
            var order = await _context.Orders.AsNoTracking().Include(x => x.OrderProducts).FirstOrDefaultAsync(x => x.Id == request.OrderId && x.CustomerEmail == customerEmail, cancellationToken);
            if (order == null)
            {
                throw new EntityNotFoundException($"Order with id = '{request.OrderId}' for customer with email = '{customerEmail}' not found.");
            }
            var products = await _productsClient.GetProductsAsync(order.OrderProducts.Select(x => x.ProductId).ToArray(), cancellationToken);
            var result = _mapper.Map<OrderViewDto>(order);
            foreach(var orderPorduct in result.OrderProducts)
            {
                orderPorduct.Product = products.FirstOrDefault(x => x.Id == orderPorduct.ProductId);
            }
            return result;
        }
    }
}
