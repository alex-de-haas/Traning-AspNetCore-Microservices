using System;
using System.Collections.Generic;
using System.Linq;

namespace Traning.AspNetCore.Microservices.Basket.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; protected set; }

        public string CustomerEmail { get; protected set; }

        protected HashSet<OrderProduct> _orderProducts;
        public IReadOnlyCollection<OrderProduct> OrderProducts => _orderProducts;

        protected Order() { }

        public Order(string customerEmail)
        {
            CustomerEmail = customerEmail;
            _orderProducts = new HashSet<OrderProduct>();
        }

        public void AddProduct(Guid productId, int quantity)
        {
            _orderProducts.Add(new OrderProduct(this, productId, quantity));
        }

        public void RemoveProduct(Guid productId)
        {
            var product = _orderProducts.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                _orderProducts.Remove(product);
            }
        }
    }
}
