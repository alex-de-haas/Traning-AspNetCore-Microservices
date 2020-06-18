using System;

namespace Traning.AspNetCore.Microservices.Basket.Domain.Entities
{
    public class CustomerBasketProduct
    {
        public Guid BasketId { get; set; }
        public Guid ProductId { get; set; }
    }
}
