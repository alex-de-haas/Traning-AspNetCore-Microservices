using Ascetic.Microservices.Application.CQRS;
using AutoMapper;
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
        private readonly ICatalogDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITracer _tracer;

        public ProductsViewQueryHandler(ICatalogDbContext context, IMapper mapper, IConfiguration configuration, ITracer tracer)
        {
            _context = context;
            _mapper = mapper;
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

            /*
            var query = _context.Products.AsQueryable().AsNoTracking();
            if (request.ProductIds.Any())
            {
                query = query.Where(x => request.ProductIds.Contains(x.Id));
            }
            return await _mapper.ProjectTo<ProductViewDto>(query).ToArrayAsync(cancellationToken);
            */
        }
    }
}
