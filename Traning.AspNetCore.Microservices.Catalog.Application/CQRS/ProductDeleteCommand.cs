using MediatR;
using System;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductDeleteCommand : IRequest
    {
        public Guid ProductId { get; set; }
    }
}
