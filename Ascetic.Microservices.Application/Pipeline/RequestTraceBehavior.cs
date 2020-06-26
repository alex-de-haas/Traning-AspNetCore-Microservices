using MediatR;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Ascetic.Microservices.Application.Pipeline
{
    public class RequestTraceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private const string DiagnosticListenerName = "RequestTrace";

        private static readonly DiagnosticSource _diagnosticSource = new DiagnosticListener(DiagnosticListenerName);

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var activity = new Activity(DiagnosticListenerName);
            activity.AddTag("component", "CQRS");
            activity.AddTag("request", typeof(TRequest).FullName);
            activity.AddTag("response", typeof(TResponse).FullName);
            activity.AddBaggage("test", "test");
            if (_diagnosticSource.IsEnabled(DiagnosticListenerName))
            {
                _diagnosticSource.StartActivity(activity, null);
            }
            try
            {
                var response = await next();
                return response;
            }
            finally
            {
                if (_diagnosticSource.IsEnabled(DiagnosticListenerName))
                {
                    _diagnosticSource.StopActivity(activity, null);
                }
            }
            /*
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
            */
        }
    }
}
