using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Ascetic.Microservices.RabbitMQ.Managers
{
    public class RabbitMqManager : IEventBusManager
    {
        private readonly ConnectionFactory _factory;
        private readonly IConfiguration _configuration;

        public RabbitMqManager(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory()
            {
                HostName = _configuration.GetValue<string>("EVENTBUS_HOSTNAME"),
                Port = _configuration.GetValue<int>("EVENTBUS_PORT"),
                UserName = _configuration.GetValue<string>("EVENTBUS_USERNAME"),
                Password = _configuration.GetValue<string>("EVENTBUS_PASSWORD"),
                VirtualHost = _configuration.GetValue<string>("EVENTBUS_VHOST"),
            };
        }

        public void Publish<T>(string queue, T model)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
            }
        }
    }
}
