using AutoMapper;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;
using Traning.AspNetCore.Microservices.Catalog.Application.CQRS;
using Traning.AspNetCore.Microservices.Catalog.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Catalog.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewDto>();
            CreateMap<ProductCreateDto, ProductCreateCommand>();
            CreateMap<ProductUpdateDto, ProductUpdateCommand>();
        }
    }
}
