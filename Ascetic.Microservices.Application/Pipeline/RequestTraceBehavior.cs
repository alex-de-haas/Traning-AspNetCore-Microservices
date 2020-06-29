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
            if (_diagnosticSource.IsEnabled(DiagnosticListenerName))
            {
                activity.AddTag("component", "CQRS");
                activity.AddTag("request", typeof(TRequest).FullName);
                activity.AddTag("response", typeof(TResponse).FullName);
                activity.AddBaggage("test", "test");
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
        }
    }
}
