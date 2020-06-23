using Ascetic.Microservices.Application.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OpenTracing;
using OpenTracing.Tag;

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
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    if (!exceptionHandlerPathFeature.Error.Data.Contains("HandledByTracer"))
                    {
                        exceptionHandlerPathFeature.Error.Data.Add("HandledByTracer", true);
                        var tracer = context.RequestServices.GetRequiredService<ITracer>();
                        Tags.Error.Set(tracer.ActiveSpan, true);
                    }

                    if (exceptionHandlerPathFeature.Error is ValidationException validationException)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            context.TraceIdentifier,
                            validationException.Message,
                            validationException.Errors
                        }));
                    }
                    else if (exceptionHandlerPathFeature.Error is EntityNotFoundException entityNotFoundException)
                    {
                        context.Response.StatusCode = 400;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            context.TraceIdentifier,
                            entityNotFoundException.Message
                        }));
                    }
                    else
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            context.TraceIdentifier,
                        }));
                    }
                });
            });
            return app;
        }
    }
}
