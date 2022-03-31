namespace document_service.Clients.MessageQueueClient;

public interface IMessageQueueClient
{
    void Publish<T>(string queueName, T message);
}