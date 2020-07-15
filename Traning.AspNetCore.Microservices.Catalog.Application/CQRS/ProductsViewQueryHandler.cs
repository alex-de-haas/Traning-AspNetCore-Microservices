using Ascetic.Microservices.Application.CQRS;
using Dapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using OpenTracing;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductsViewQueryHandler : IRequestHandler<ProductsViewQuery, ProductViewDto[]>, IQueryHandler<ProductsViewQuery, ProductViewDto[]>
    {
        private readonly IConfiguration _configuration;
        private readonly ITracer _tracer;

        public ProductsViewQueryHandler(IConfiguration configuration, ITracer tracer)
        {
            _configuration = configuration;
            _tracer = tracer;
        }

        public async Task<ProductViewDto[]> Handle(ProductsViewQuery request, CancellationToken cancellationToken)
        {
            
            using (var connection = new SqlConnection(_configuration["DATABASE"]))
            {
                await connection.OpenAsync(cancellationToken);
                var query = "SELECT * FROM Products";
                if (request.ProductIds.Any())
                {
                    query += " WHERE Id IN @ProductIds";
                }
                using (var scope = _tracer.BuildSpan($"Products Select").StartActive(finishSpanOnDispose: true))
                {
                    var result = await connection.QueryAsync<ProductViewDto>(query, request);
                    return result.ToArray();
                }
            }
        }
    }
}
