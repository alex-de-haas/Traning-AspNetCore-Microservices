using Ascetic.Microservices.RabbitMQ.Managers;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;
using Traning.AspNetCore.Microservices.Catalog.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Guid>
    {
        private readonly ICatalogDbContext _context;
        private readonly IEventBusManager _eventBusManager;
        private readonly IMapper _mapper;

        public ProductCreateCommandHandler(ICatalogDbContext context, IEventBusManager eventBusManager, IMapper mapper)
        {
            _context = context;
            _eventBusManager = eventBusManager;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Description);
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            _eventBusManager.Publish(_mapper.Map<ProductViewDto>(product), "catalog.product-created");
            return product.Id;
        }
    }
}
