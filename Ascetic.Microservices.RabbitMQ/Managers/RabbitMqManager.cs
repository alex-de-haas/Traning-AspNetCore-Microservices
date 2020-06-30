using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace Ascetic.Microservices.RabbitMQ.Managers
{
    public class RabbitMqManager : IEventBusManager
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMqManager(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(T model, string queue)
        {
            var channel = _objectPool.Get();
            try
            {
                channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                var message = JsonConvert.SerializeObject(model);
                var sendBytes = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                channel.BasicPublish("", queue, properties, sendBytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }
    }
}
