using Ascetic.Microservices.API.DiagnosticObservers;
using Ascetic.Microservices.Application.Pipeline;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTracing.Util;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJaeger(this IServiceCollection services)
        {
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var config = Jaeger.Configuration.FromEnv(loggerFactory);
                var tracer = config.GetTracer();
                if (!GlobalTracer.IsRegistered())
                {
                    GlobalTracer.Register(tracer);
                }
                return tracer;
            });
            return services;
        }

        public static IServiceCollection AddSerilogLogging(this IServiceCollection services)
        {
            var loggerConfiguration = new LoggerConfiguration().Enrich.FromLogContext();
#if DEBUG
            loggerConfiguration.WriteTo.Console();
#else
            loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());
#endif
            Log.Logger = loggerConfiguration.CreateLogger();
            return services.AddLogging(config =>
            {
                config.ClearProviders();
                config.AddSerilog(dispose: true);
            });
        }

        public static IServiceCollection AddPipelineBehavior(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestTraceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            return services;
        }

        public static void AddDiagnosticObserver<TDiagnosticObserver>(this IServiceCollection services) where TDiagnosticObserver : DiagnosticObserverBase
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<DiagnosticObserverBase, TDiagnosticObserver>());
        }
    }
}
