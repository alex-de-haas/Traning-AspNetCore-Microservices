using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.Microservices.Application.Pipeline
{
    public class RequestTraceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ITracer _tracer;
        private readonly ILogger<RequestTraceBehavior<TRequest, TResponse>> _logger;

        public RequestTraceBehavior(ITracer tracer, ILogger<RequestTraceBehavior<TRequest, TResponse>> logger)
        {
            _tracer = tracer;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var scope = _tracer.BuildSpan($"CQRS: {typeof(TRequest).FullName}").StartActive(finishSpanOnDispose: true))
            {
                try
                {
                    var response = await next();
                    return response;
                }
                catch (Exception e)
                {
                    Tags.Error.Set(scope.Span, true);
                    _logger.LogError(e, "CQRS error");
                    throw;
                }
            }
        }
    }
}
