using Microsoft.Extensions.DiagnosticAdapter;
using OpenTracing;
using OpenTracing.Tag;

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
        public void OnHandleStart()
        {
            _tracer.BuildSpan(DiagnosticListenerName)
                .WithTag(Tags.SpanKind, Tags.SpanKindClient)
                .WithTag(Tags.Component, "Ascetic.Microservices.Application")
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
    }
}
