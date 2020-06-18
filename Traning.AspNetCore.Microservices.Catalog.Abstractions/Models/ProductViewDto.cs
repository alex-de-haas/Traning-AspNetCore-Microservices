using System;

namespace Traning.AspNetCore.Microservices.Catalog.Abstractions.Models
{
    public class ProductViewDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
