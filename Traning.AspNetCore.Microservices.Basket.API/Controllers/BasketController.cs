using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Basket.Abstractions.Models;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;

namespace Traning.AspNetCore.Microservices.Basket.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BasketController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet(Name = nameof(GetBasketAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<BasketViewDto>> GetBasketAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new BasketViewQuery(), cancellationToken);
            return Ok(result);
        }

        [HttpPost(Name = nameof(UpdateBasketAsync))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateBasketAsync([FromBody] BasketUpdateDto model, CancellationToken cancellationToken = default)
        {
            var command = _mapper.Map<BasketUpdateCommand>(model);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
