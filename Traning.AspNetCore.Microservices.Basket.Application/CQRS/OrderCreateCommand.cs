using MediatR;
using System;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrderCreateCommand : OrderCreateDto, IRequest<Guid>
    {
    }
}
