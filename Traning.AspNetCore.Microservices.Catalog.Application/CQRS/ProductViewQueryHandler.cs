using AutoMapper;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductViewQueryHandler : IRequestHandler<ProductViewQuery, ProductViewDto>
    {
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductViewQueryHandler(ICatalogDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ProductViewDto> Handle(ProductViewQuery request, CancellationToken cancellationToken)
        {
            using (var connection = new SqlConnection(_configuration["DATABASE"]))
            {
                await connection.OpenAsync(cancellationToken);
                var query = "SELECT * FROM Products WHERE Id = @ProductId";
                var result = await connection.QueryFirstOrDefaultAsync<ProductViewDto>(query, request);
                return result;
            }
            /*
            var product = await _context.Products.FindAsync(request.ProductId);
            return _mapper.Map<ProductViewDto>(product);
            */
        }
    }
}
