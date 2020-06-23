using System;

namespace Traning.AspNetCore.Microservices.Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        protected Product() { }

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
