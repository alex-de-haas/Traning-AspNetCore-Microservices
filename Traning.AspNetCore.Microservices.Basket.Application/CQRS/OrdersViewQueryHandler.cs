using Ascetic.Microservices.Application.Extensions;
using Ascetic.Microservices.Application.Managers;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrdersViewQueryHandler : IRequestHandler<OrdersViewQuery, OrderViewDto[]>
    {
        private readonly IBasketDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserContextManager _userContextManager;

        public OrdersViewQueryHandler(IBasketDbContext context, IMapper mapper, IUserContextManager userContextManager)
        {
            _context = context;
            _mapper = mapper;
            _userContextManager = userContextManager;
        }

        public async Task<OrderViewDto[]> Handle(OrdersViewQuery request, CancellationToken cancellationToken)
        {
            var customerEmail = _userContextManager.GetCurrentUserEmail();
            var query = _context.Orders.AsNoTracking().Where(x => x.CustomerEmail == customerEmail);
            return await _mapper.ProjectTo<OrderViewDto>(query).ToArrayAsync(cancellationToken);
        }
    }
}
