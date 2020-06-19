using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductViewQueryHandler : IRequestHandler<ProductViewQuery, ProductViewDto>
    {
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductViewQueryHandler(ICatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductViewDto> Handle(ProductViewQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                throw new ApplicationException($"Product with id = '{request.ProductId}' not found.");
            }
            return _mapper.Map<ProductViewDto>(product);
        }
    }
}
