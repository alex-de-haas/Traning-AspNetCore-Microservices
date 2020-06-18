using FluentValidation;
using MediatR;
using OpenTracing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.AspNetCore.Microservices.Pipeline
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ITracer _tracer;
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(ITracer tracer, IEnumerable<IValidator<TRequest>> validators)
        {
            _tracer = tracer;
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var validationSpan = _tracer.BuildSpan("Data Validation").Start();
            var context = new ValidationContext(request);
            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();
            validationSpan.Finish();
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
