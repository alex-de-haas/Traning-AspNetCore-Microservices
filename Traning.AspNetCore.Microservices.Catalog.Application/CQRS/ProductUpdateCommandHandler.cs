using AutoMapper;
using MediatR;
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
            var entity = await _context.Products.FindAsync(request.ProductId);
            _mapper.Map(request, entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
