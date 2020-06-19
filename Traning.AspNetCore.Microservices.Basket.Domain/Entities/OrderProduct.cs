using System;

namespace Traning.AspNetCore.Microservices.Basket.Domain.Entities
{
    public class OrderProduct
    {
        public Guid OrderId { get; private set; }
        public Order Order { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        private OrderProduct() { }

        public OrderProduct(Order order, Guid productId, int quantity)
        {
            OrderId = order.Id;
            Order = order;
            ProductId = productId;
            Update(quantity);
        }

        public void Update(int quantity)
        {
            Quantity = quantity;
        }
    }
}
