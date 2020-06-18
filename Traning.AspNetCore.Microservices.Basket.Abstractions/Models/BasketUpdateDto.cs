using System;

namespace Traning.AspNetCore.Microservices.Basket.Abstractions.Models
{
    public class BasketUpdateDto
    {
        public Guid[] ProductIds { get; set; }
    }
}
