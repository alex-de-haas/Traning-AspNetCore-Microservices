using System;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class OrderViewDto
    {
        public Guid Id { get; set; }
        public OrderProductViewDto[] OrderProducts { get; set; }
    }
}
