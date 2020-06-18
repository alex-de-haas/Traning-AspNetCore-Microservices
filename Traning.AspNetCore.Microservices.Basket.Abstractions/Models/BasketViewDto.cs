using System;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class BasketViewDto
    {
        public Guid Id { get; set; }
        public ProductViewDto[] Products { get; set; }
    }
}
