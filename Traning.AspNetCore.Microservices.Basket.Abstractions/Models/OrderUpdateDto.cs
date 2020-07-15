using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class OrderUpdateDto
    {
        public OrderStatus Status { get; set; }
    }
}
