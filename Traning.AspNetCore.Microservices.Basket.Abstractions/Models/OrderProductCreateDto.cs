using System;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class OrderProductCreateDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
