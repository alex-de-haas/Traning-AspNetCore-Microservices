using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductUpdateCommandHandler : IRequestHandler<ProductUpdateCommand>
    {
        private readonly ICatalogDbContext _context;

        public ProductUpdateCommandHandler(ICatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
            product.Update(request.Name, request.Description);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
