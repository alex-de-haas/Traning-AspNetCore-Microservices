using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.Microservices.Application.Pipeline
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ITracer _tracer;
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<RequestValidationBehavior<TRequest, TResponse>> _logger;

        public RequestValidationBehavior(ITracer tracer, IEnumerable<IValidator<TRequest>> validators, ILogger<RequestValidationBehavior<TRequest, TResponse>> logger)
        {
            _tracer = tracer;
            _validators = validators;
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            using (var scope = _tracer.BuildSpan("Validation").StartActive(finishSpanOnDispose: true))
            {
                try
                {
                    var context = new ValidationContext(request);
                    var failures = _validators
                        .Select(v => v.Validate(context))
                        .SelectMany(result => result.Errors)
                        .Where(f => f != null)
                        .ToList();
                    if (failures.Count != 0)
                    {
                        throw new ValidationException(failures);
                    }
                    return next();
                }
                catch (Exception e)
                {
                    if (!e.Data.Contains("HandledByTracer"))
                    {
                        e.Data.Add("HandledByTracer", true);
                        Tags.Error.Set(scope.Span, true);
                        _logger.LogError(e, "Validation error");
                    }
                    throw;
                }
            }
        }
    }
}
