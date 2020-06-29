using Ascetic.Microservices.Application.Extensions;
using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.Microservices.Application.Pipeline
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private const string DiagnosticListenerName = "RequestValidation";

        private static readonly DiagnosticSource _diagnosticSource = new DiagnosticListener(DiagnosticListenerName);

        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _diagnosticSource.WriteIfEnabled($"{DiagnosticListenerName}.HandleStart", new { ValidationsCount = _validators.Count() });
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
                    _diagnosticSource.WriteIfEnabled($"{DiagnosticListenerName}.HandleError", new { Failures = failures });
                    throw new ValidationException(failures);
                }
            }
            finally
            {
                _diagnosticSource.WriteIfEnabled($"{DiagnosticListenerName}.HandleEnd", new { });
            }
            return next();
        }
    }
}
