using Ascetic.AspNetCore.Microservices.DelegatingHandlers;
using Ascetic.AspNetCore.Microservices.Managers;
using Ascetic.Microservices.Application.Managers;
using AutoMapper;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Jaeger.Thrift;
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
using Traning.AspNetCore.Microservices.Basket.API.Infrastructure;
using Traning.AspNetCore.Microservices.Basket.Application;
using Traning.AspNetCore.Microservices.Basket.Application.CQRS;
using Traning.AspNetCore.Microservices.Basket.Application.Mapping;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Clients;

namespace Traning.AspNetCore.Microservices.Basket.API
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
                .AddDbContext<IBasketDbContext, BasketDbContext>(options =>
                {
                    options.UseSqlServer(Configuration["DATABASE"] ?? Configuration.GetConnectionString("BasketDbContext"));
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
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BasketUpdateCommandValidation>());
            services
                .AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket", Version = "v1" });
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
            services.AddOpenTracing();
            services.AddJaeger();
            services.AddPipelineBehavior();
            services.AddHttpContextAccessor();

            services
                .AddHealthChecks()
                .AddSqlServer(Configuration["DATABASE"], tags: new[] { "ready" });

            services
                .AddHealthChecksUI(setupSettings: setup =>
                {
                    setup.AddHealthCheckEndpoint("endpoint1", "/health/json");
                })
                .AddInMemoryStorage();

            services.AddAutoMapper(typeof(BasketProfile).Assembly);
            services.AddMediatR(typeof(BasketViewQueryHandler).GetTypeInfo().Assembly);

            services.AddScoped<IUserContextManager, UserContextManager>();

            services.AddTransient<AuthorizationHeaderHandler>();
            services
                .AddHttpClient<IProductsClient, ProductsClient>(client => client.BaseAddress = new Uri(Configuration["API_URL_CATALOG"]))
                .AddHttpMessageHandler<AuthorizationHeaderHandler>();
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
            app.UseHealthChecks("/health/json", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
            app.UseSwagger(options => options.PreSerializeFilters.Add((swagger, request) =>
            {
                swagger.Servers.Add(new OpenApiServer { Url = $"{request.Scheme}://{request.Host.Value}{Configuration.GetValue("BASE_URL", string.Empty)}" });
            }));
            app.UseSwaggerUI(options =>
            {
                options.OAuthClientId(Configuration["AzureAd:ClientId"]);
                options.SwaggerEndpoint("v1/swagger.json", "Basket");
            });
        }
    }
}
