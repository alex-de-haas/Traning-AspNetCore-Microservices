using System;

namespace Traning.AspNetCore.Microservices.Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
