using System.Diagnostics;

namespace Ascetic.Microservices.Application.Extensions
{
    public static class DiagnosticSourceExtensions
    {
        public static void WriteIfEnabled(this DiagnosticSource diagnosticSource, string eventName, object eventArgs)
        {
            if (diagnosticSource.IsEnabled(eventName))
            {
                diagnosticSource.Write(eventName, eventArgs);
            }
        }
    }
}
