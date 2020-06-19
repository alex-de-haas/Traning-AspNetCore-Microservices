using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;

namespace Traning.AspNetCore.Microservices.Basket.API.Controllers
{
    public partial class OrdersController : ControllerBase
    {
        [HttpPost("{orderId}/products", Name = nameof(CreateOrderProductAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> CreateOrderProductAsync(Guid orderId, [FromBody] OrderProductCreateDto model, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<OrderProductCreateCommand>(model);
            command.OrderId = orderId;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpPut("{orderId}/products/{productId}", Name = nameof(UpdateOrderProductAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateOrderProductAsync(Guid orderId, Guid productId, [FromBody] OrderProductUpdateDto model, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<OrderProductUpdateCommand>(model);
            command.OrderId = orderId;
            command.ProductId = productId;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{orderId}/products/{productId}", Name = nameof(UpdateOrderProductAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOrderProductAsync(Guid orderId, Guid productId, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new OrderProductDeleteCommand
            {
                OrderId = orderId,
                ProductId = productId
            }, cancellationToken);
            return NoContent();
        }
    }
}
