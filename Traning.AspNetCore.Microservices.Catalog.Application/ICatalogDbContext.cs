using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Catalog.Application
{
    public interface ICatalogDbContext
    {
        DbSet<Product> Products { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
