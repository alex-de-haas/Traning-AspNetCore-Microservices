using System;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class OrderViewDto
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
        public OrderProductViewDto[] OrderProducts { get; set; }
    }
}
