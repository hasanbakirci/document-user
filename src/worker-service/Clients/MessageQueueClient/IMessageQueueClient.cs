namespace worker_service.Clients.MessageQueueClient;

public interface IMessageQueueClient
{
    void Subscribe<T>(string queueName, Action<T> callback);
}