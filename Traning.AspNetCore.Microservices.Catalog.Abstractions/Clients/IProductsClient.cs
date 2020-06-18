using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients
{
    public interface IProductsClient
    {
        Task<ProductViewDto[]> GetProductsAsync(Guid[] productIds = default, CancellationToken cancellationToken = default);
        Task<ProductViewDto> GetProductAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<Guid> CreateProductAsync(ProductCreateDto product, CancellationToken cancellationToken = default);
        Task UpdateProductAsync(ProductViewDto product, CancellationToken cancellationToken = default);
        Task DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default);
    }
}
