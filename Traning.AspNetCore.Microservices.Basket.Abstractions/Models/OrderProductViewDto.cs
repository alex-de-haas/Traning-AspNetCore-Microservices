using System;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class OrderProductViewDto
    {
        public Guid ProductId { get; set; }
        public ProductViewDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
