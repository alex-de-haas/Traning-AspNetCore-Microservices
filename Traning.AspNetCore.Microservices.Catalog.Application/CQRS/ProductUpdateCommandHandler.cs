using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand>
    {
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductUpdateCommandHandler(ICatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ApplicationException($"Product with id = '{request.ProductId}' not found.");
            }
            _mapper.Map(request, product);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
