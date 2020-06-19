using MediatR;
using System;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderProductUpdateCommand : OrderProductUpdateDto, IRequest
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
    }
}
