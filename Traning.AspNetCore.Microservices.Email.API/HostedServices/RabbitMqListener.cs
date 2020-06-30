using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Traning.AspNetCore.Microservices.Catalog.Abstractions.Models;
using Traning.AspNetCore.Microservices.Email.API.Managers;

namespace Traning.AspNetCore.Microservices.Email.API.HostedServices
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private IConnection _connection;
        private IModel _channel;

        public RabbitMqListener(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory()
            {
                HostName = configuration.GetValue<string>("EVENTBUS_HOSTNAME"),
                Port = configuration.GetValue<int>("EVENTBUS_PORT"),
                UserName = configuration.GetValue<string>("EVENTBUS_USERNAME"),
                Password = configuration.GetValue<string>("EVENTBUS_PASSWORD"),
                VirtualHost = configuration.GetValue<string>("EVENTBUS_VHOST"),
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "catalog.product-created", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);
            //_connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var product = JsonConvert.DeserializeObject<ProductViewDto>(message);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mailManager = scope.ServiceProvider.GetRequiredService<IMailManager>();
                    mailManager.Send("test@mail.com", "Product Created", $"Product Created: {product.Name}");
                }
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            //consumer.Shutdown += OnConsumerShutdown;
            //consumer.Registered += OnConsumerRegistered;
            //consumer.Unregistered += OnConsumerUnregistered;
            //consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume("catalog.product-created", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
