using Ascetic.Microservices.Application.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand>
    {
        private readonly ICatalogDbContext _context;

        public ProductDeleteCommandHandler(ICatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id = '{request.ProductId}' not found.");
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
