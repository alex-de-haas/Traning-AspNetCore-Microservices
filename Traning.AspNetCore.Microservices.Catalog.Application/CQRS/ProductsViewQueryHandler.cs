using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductsViewQueryHandler : IRequestHandler<ProductsViewQuery, ProductViewDto[]>
    {
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductsViewQueryHandler(ICatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductViewDto[]> Handle(ProductsViewQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Products.AsQueryable().AsNoTracking();
            if (request.ProductIds.Any())
            {
                query = query.Where(x => request.ProductIds.Contains(x.Id));
            }
            return await _mapper.ProjectTo<ProductViewDto>(query).ToArrayAsync(cancellationToken);
        }
    }
}
