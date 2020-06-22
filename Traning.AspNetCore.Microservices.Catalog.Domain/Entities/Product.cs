using System;

namespace Traning.AspNetCore.Microservices.Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }

        private Product() { }

        public Product(string name, string description)
        {
            Update(name, description);
        }

        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
