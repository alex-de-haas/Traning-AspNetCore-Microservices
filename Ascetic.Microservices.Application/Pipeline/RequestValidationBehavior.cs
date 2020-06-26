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
            if (_diagnosticSource.IsEnabled($"{DiagnosticListenerName}.HandleStart"))
            {
                _diagnosticSource.Write($"{DiagnosticListenerName}.HandleStart", new { });
            }
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
            }
            finally
            {
                if (_diagnosticSource.IsEnabled($"{DiagnosticListenerName}.HandleEnd"))
                {
                    _diagnosticSource.Write($"{DiagnosticListenerName}.HandleEnd", new { });
                }
            }
            return next();
        }
    }
}
