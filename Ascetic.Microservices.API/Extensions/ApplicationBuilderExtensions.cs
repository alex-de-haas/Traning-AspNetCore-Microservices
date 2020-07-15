using Ascetic.Microservices.API.DiagnosticObservers;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Tag;
using System.Diagnostics;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseMicroserviceExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var tracer = context.RequestServices.GetRequiredService<ITracer>();
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature.Error is ValidationException validationException)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            tracer.ActiveSpan.Context.TraceId,
                            validationException.Message,
                            validationException.Errors
                        }));
                    }
                    else
                    {
                        tracer.ActiveSpan.SetTag(Tags.Error, true);
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            tracer.ActiveSpan.Context.TraceId,
                        }));
                    }
                });
            });
            return app;
        }

        public static IApplicationBuilder UseDiagnosticObservers(this IApplicationBuilder app)
        {
            var diagnosticObservers = app.ApplicationServices.GetServices<DiagnosticObserverBase>();
            foreach (var diagnosticObserver in diagnosticObservers)
            {
                DiagnosticListener.AllListeners.Subscribe(diagnosticObserver);
            }
            return app;
        }
    }
}
