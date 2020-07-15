using AutoMapper;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;
using Traning.AspNetCore.Microservices.Basket.Domain.Entities;

namespace Traning.AspNetCore.Microservices.Basket.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderViewDto>();
            CreateMap<OrderCreateDto, OrderCreateCommand>();
            CreateMap<OrderUpdateDto, OrderUpdateCommand>();
            CreateMap<OrderProductCreateDto, OrderProductCreateCommand>();
            CreateMap<OrderProductUpdateDto, OrderProductUpdateCommand>();
        }
    }
}
