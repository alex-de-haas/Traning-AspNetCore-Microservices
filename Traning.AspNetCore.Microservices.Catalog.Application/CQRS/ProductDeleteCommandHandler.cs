using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Traning.AspNetCore.Microservices.Catalog.Application.CQRS
{
    public class ProductDeleteCommandHandler : IRequestHandler<ProductDeleteCommand>
    {
        public Task<Unit> Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
