using MediatR;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Application.CQRS
{
    public class OrdersViewQuery : IRequest<OrderViewDto[]>
    {
    }
}
