namespace Ascetic.Microservices.RabbitMQ.Managers
{
    public interface IEventBusManager
    {
        void Publish<T>(string queue, T model); 
    }
}
