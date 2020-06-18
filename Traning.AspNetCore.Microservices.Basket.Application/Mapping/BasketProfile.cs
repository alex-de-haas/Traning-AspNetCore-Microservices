using AutoMapper;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Application.Mapping
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketViewDto>()
                .ForMember(d => d.Products, o => o.Ignore());
            CreateMap<BasketUpdateDto, BasketUpdateCommand>();
        }
    }
}
