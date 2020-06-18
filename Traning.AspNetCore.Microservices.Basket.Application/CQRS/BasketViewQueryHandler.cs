using Ascetic.Microservices.Application.Managers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class BasketViewQueryHandler : IRequestHandler<BasketViewQuery, BasketViewDto>
    {
        private readonly IBasketDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextManager _userContextManager;
        private readonly IProductsClient _productsClient;

        public BasketViewQueryHandler(IBasketDbContext context, IMapper mapper, IUserContextManager userContextManager, IProductsClient productsClient)
        {
            _context = context;
            _mapper = mapper;
            _userContextManager = userContextManager;
            _productsClient = productsClient;
        }

        public async Task<BasketViewDto> Handle(BasketViewQuery request, CancellationToken cancellationToken)
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
            }
            var result = _mapper.Map<BasketViewDto>(basket);
            result.Products = await _productsClient.GetProductsAsync(basket.Products.Select(x => x.ProductId).ToArray(), cancellationToken);
            return result;
        }
    }
}
