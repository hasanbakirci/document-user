namespace worker_service.Clients.MessageQueueClient;

public static class RabbitMQHelper
{
    public static string LoggerQueue => "Log-Document-Queue";
    public static string LoggerExchange => "Log-Document-Exchange";
}