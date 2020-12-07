using System;

namespace Traning.AspNetCore.Microservices.Catalog.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public string Description { get; protected set; }

        public bool IsDeleted { get; protected set; }

        protected Product() { }

        public Product(string name, string description)
        {
            Update(name, description);
        }

        public void Update(string name, string description)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

            Name = name;
            Description = description;
        }

        public void Delete()
        {
            if (IsDeleted) throw new InvalidOperationException("Product already deleted.");

            IsDeleted = true;
        }
    }
}
