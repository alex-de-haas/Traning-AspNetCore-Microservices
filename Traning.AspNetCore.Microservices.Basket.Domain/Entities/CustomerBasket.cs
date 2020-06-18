using System;
using System.Collections.Generic;

namespace Traning.AspNetCore.Microservices.Basket.Domain.Entities
{
    public class CustomerBasket
    {
        public Guid Id { get; set; }
        public string CustomerEmail { get; set; }
        public ICollection<CustomerBasketProduct> Products { get; set; }
    }
}
