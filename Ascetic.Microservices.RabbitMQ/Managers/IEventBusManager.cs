namespace Ascetic.Microservices.RabbitMQ.Managers
{
    public interface IEventBusManager
    {
        void Publish<T>(T model, string queue); 
    }
}
