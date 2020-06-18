using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProductCreateCommandHandler(ICatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request);
            _context.Products.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity.Id;
        }
    }
}
