using System;
using MediatR;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderUpdateCommand : OrderUpdateDto, IRequest
    {
        public Guid OrderId { get; set; }
    }
}
