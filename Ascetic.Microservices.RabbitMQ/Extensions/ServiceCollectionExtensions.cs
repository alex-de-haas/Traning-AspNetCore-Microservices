using Ascetic.Microservices.RabbitMQ;
using Ascetic.Microservices.RabbitMQ.Managers;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, Action<RabbitOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitModelPooledObjectPolicy>();
            services.AddSingleton<IEventBusManager, RabbitMqManager>();
            return services;
        }
    }
}
