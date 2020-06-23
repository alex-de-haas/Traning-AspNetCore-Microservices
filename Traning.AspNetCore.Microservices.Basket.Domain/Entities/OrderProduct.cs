using System;

namespace Traning.AspNetCore.Microservices.Basket.Domain.Entities
{
    public class OrderProduct
    {
        public Guid OrderId { get; protected set; }

        public Order Order { get; protected set; }

        public Guid ProductId { get; protected set; }

        public int Quantity { get; protected set; }

        protected OrderProduct() { }

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
