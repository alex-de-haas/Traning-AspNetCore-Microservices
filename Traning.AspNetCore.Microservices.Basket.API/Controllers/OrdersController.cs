using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;

namespace Traning.AspNetCore.Microservices.Basket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public partial class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetOrdersAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<OrderViewDto[]>> GetOrdersAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new OrdersViewQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpGet("{orderId}", Name = nameof(GetOrderAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<OrderViewDto>> GetOrderAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new OrderViewQuery 
            { 
                OrderId = orderId 
            }, cancellationToken);
            return Ok(result);
        }

        [HttpPost(Name = nameof(CreateOrderAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateOrderAsync([FromBody] OrderCreateDto model, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<OrderCreateCommand>(model);
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtRoute(nameof(GetOrderAsync), new { orderId = result }, result);
        }

        [HttpPut("{orderId}", Name = nameof(UpdateOrderAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task UpdateOrderAsync(Guid orderId, [FromBody] OrderUpdateDto model, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<OrderUpdateCommand>(model);
            command.OrderId = orderId;
            await _mediator.Send(command, cancellationToken);
            Response.StatusCode = StatusCodes.Status204NoContent;
        }
    }
}
