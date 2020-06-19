using AutoMapper;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Application.Mapping
{
    public class OrderProductProfile : Profile
    {
        public OrderProductProfile()
        {
            CreateMap<OrderProduct, OrderProductViewDto>();
        }
    }
}
