using FluentValidation.Results;
using Microsoft.Extensions.DiagnosticAdapter;
using OpenTracing;
using OpenTracing.Tag;
using System.Collections.Generic;

namespace Ascetic.Microservices.API.DiagnosticObservers
{
    public sealed class RequestDiagnosticObserver : DiagnosticObserverBase
    {
        public const string DiagnosticListenerName = "RequestValidation";
        private readonly ITracer _tracer;

        public RequestDiagnosticObserver(ITracer tracer)
        {
            _tracer = tracer;
        }

        protected override bool IsMatch(string name)
        {
            return name == DiagnosticListenerName;
        }

        [DiagnosticName(DiagnosticListenerName + ".HandleStart")]
        public void OnHandleStart(int validationsCount)
        {
            _tracer.BuildSpan(DiagnosticListenerName)
                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                .WithTag(Tags.Component, "Ascetic.Microservices.Application")
                .WithTag("validation.count", validationsCount)
                .StartActive();
        }

        [DiagnosticName(DiagnosticListenerName + ".HandleEnd")]
        public void OnHandleEnd()
        {
            var scope = _tracer.ScopeManager.Active;
            if (scope != null)
            {
                scope.Dispose();
            }
        }

        [DiagnosticName(DiagnosticListenerName + ".HandleError")]
        public void OnHandleError(IEnumerable<ValidationFailure> failures)
        {
            var scope = _tracer.ScopeManager.Active;
            if (scope != null)
            {
                scope.Span.SetTag(Tags.Error, true);
                foreach (var failure in failures)
                {
                    scope.Span.Log(failure.ErrorMessage);
                }
                scope.Dispose();
            }
        }
    }
}
