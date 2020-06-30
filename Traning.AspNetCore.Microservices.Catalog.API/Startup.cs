using Ascetic.Microservices.API.DiagnosticObservers;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Traning.AspNetCore.Microservices.Catalog.API.Infrastructure;
using Traning.AspNetCore.Microservices.Catalog.Application;
using Traning.AspNetCore.Microservices.Catalog.Application.CQRS;
using Traning.AspNetCore.Microservices.Catalog.Application.Mapping;

namespace Traning.AspNetCore.Microservices.Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<ICatalogDbContext, CatalogDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["DATABASE"] ?? Configuration.GetConnectionString("CatalogDbContext"));
#if DEBUG
                    options.EnableSensitiveDataLogging();
#endif
                });
            services
                .AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.Configure<JwtBearerOptions>(AzureADDefaults.JwtBearerAuthenticationScheme, options =>
            {
                options.Authority += "/v2.0";
                options.TokenValidationParameters.ValidAudiences = new[] { options.Audience, $"api://{options.Audience}" };
                options.TokenValidationParameters.ValidateIssuer = false;
            });
            services
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductCreateCommandValidator>());
            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog", Version = "v1" });
                    var assembly = Assembly.GetExecutingAssembly();
                    var apiXmlPath = Path.ChangeExtension(assembly.Location, "xml");
                    options.IncludeXmlComments(apiXmlPath, includeControllerXmlComments: true);
                    options.AddSecurityDefinition("aad-jwt", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri($"{Configuration["AzureAd:Instance"] + Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
                                TokenUrl = new Uri($"{Configuration["AzureAd:Instance"] + Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "openid", "Sign In Permissions" },
                                    { "profile", "User Profile Permissions" },
                                    { $"api://{Configuration["AzureAd:ClientId"]}/user_impersonation", "Application API Permissions" }
                                }
                            }
                        }
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "aad-jwt" }
                            },
                            new string[0]
                        }
                    });
                });

            services.AddSerilogLogging();
            services.AddDiagnosticObserver<RequestDiagnosticObserver>();
            services.AddOpenTracing(builder => builder.ConfigureGenericDiagnostics(options =>
            {
                options.IgnoredListenerNames.Add(RequestDiagnosticObserver.DiagnosticListenerName);
            }));
            services.AddJaeger();
            services.AddPipelineBehavior();
            services.AddHttpContextAccessor();

            services
                .AddHealthChecks()
                .AddSqlServer(Configuration["DATABASE"], tags: new[] { "ready" });

            /*
            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    setup.AddHealthCheckEndpoint("endpoint1", "/health/json");
                })
                .AddInMemoryStorage();
            */

            services.AddAutoMapper(typeof(ProductProfile).Assembly);
            services.AddMediatR(typeof(ProductsViewQueryHandler).GetTypeInfo().Assembly);

            services.AddRabbitMqEventBus(options =>
            {
                options.HostName = Configuration.GetValue<string>("EVENTBUS_HOSTNAME");
                options.Port = Configuration.GetValue<int>("EVENTBUS_PORT");
                options.UserName = Configuration.GetValue<string>("EVENTBUS_USERNAME");
                options.Password = Configuration.GetValue<string>("EVENTBUS_PASSWORD");
                options.VHost = Configuration.GetValue<string>("EVENTBUS_VHOST");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMicroserviceExceptionHandler();
            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                Predicate = check => check.Tags.Contains("ready")
            });
            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                Predicate = _ => true
            });
            /*
            app.UseHealthChecks("/health/json", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            */
            app.UseDiagnosticObservers();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHealthChecksUI();
            });
            app.UseSwagger(options => options.PreSerializeFilters.Add((swagger, request) =>
            {
                swagger.Servers.Add(new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}{Configuration.GetValue("BASE_URL", string.Empty)}" });
            }));
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId(Configuration["AzureAd:ClientId"]);
                options.SwaggerEndpoint("v1/swagger.json", "Catalog");
            });
        }
    }
}
