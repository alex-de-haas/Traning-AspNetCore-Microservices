using MediatR;
using System;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductDeleteCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}
