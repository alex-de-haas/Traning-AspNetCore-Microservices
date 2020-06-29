using Ascetic.Microservices.RabbitMQ.Managers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Guid>
    {
        private readonly ICatalogDbContext _context;
        private readonly IEventBusManager _eventBusManager;

        public ProductCreateCommandHandler(ICatalogDbContext context, IEventBusManager eventBusManager)
        {
            _context = context;
            _eventBusManager = eventBusManager;
        }

        public async Task<Guid> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var product = new Product(request.Name, request.Description);
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);
            _eventBusManager.Publish("product-created", product);
            return product.Id;
        }
    }
}
