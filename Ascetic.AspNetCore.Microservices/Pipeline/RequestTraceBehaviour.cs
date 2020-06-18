using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.AspNetCore.Microservices.Pipeline
{
    public class RequestTraceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ITracer _tracer;
        private readonly ILogger<RequestTraceBehaviour<TRequest, TResponse>> _logger;

        public RequestTraceBehaviour(ITracer tracer, ILogger<RequestTraceBehaviour<TRequest, TResponse>> logger)
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
