using Ascetic.Microservices.Application.CQRS;
using MediatR;
using System;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductsViewQuery : IRequest<ProductViewDto[]>, IQuery<ProductViewDto[]>
    {
        public Guid[] ProductIds { get; set; }
    }
}
