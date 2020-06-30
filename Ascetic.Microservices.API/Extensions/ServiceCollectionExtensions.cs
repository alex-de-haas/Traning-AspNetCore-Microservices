using Ascetic.Microservices.API.DiagnosticObservers;
using Ascetic.Microservices.Application.Pipeline;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using Petabridge.Tracing.Zipkin;
using Serilog;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJaeger(this IServiceCollection services)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var config = Jaeger.Configuration.FromIConfiguration(loggerFactory, configuration);
                var tracer = config.GetTracer();
                if (!GlobalTracer.IsRegistered())
                {
                    GlobalTracer.Register(tracer);
                }
                return tracer;
            });
            return services;
        }

        public static IServiceCollection AddZipkin(this IServiceCollection services)
        {
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                //var tracer = new ZipkinTracer(new ZipkinTracerOptions(new Endpoint(configuration["ZIPKIN_SERVICE_NAME"]), ZipkinKafkaSpanReporter.Create(new ZipkinKafkaReportingOptions(new[] { configuration["ZIPKIN_URL"] }, debugLogging: true))));
                var tracer = new ZipkinTracer(new ZipkinTracerOptions(configuration["ZIPKIN_URL"], configuration["ZIPKIN_SERVICE_NAME"], debug: true));
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
            services.TryAddTransient<DiagnosticObserverBase, TDiagnosticObserver>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<DiagnosticObserverBase, TDiagnosticObserver>());
        }
    }
}
