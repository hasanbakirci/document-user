using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace document_service.Clients.MessageQueueClient;

public class RabbitMQClient : IMessageQueueClient
{
    private readonly IConfiguration _configuration;
    private readonly RabbitMQSettings _rabbitMQSettings;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _rabbitMQSettings = _configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettings>();
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQSettings.Hostname,
            UserName = _rabbitMQSettings.Username,
            Password = _rabbitMQSettings.Password
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(RabbitMQHelper.LoggerExchange,ExchangeType.Direct,true);
        _channel.QueueDeclare(RabbitMQHelper.LoggerQueue, false, false, false, null);
        _channel.QueueBind(RabbitMQHelper.LoggerQueue,RabbitMQHelper.LoggerExchange,RabbitMQHelper.LoggerQueue);

    }
    public void Publish<T>(string queueName, T message)
    {
        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
        _channel.BasicPublish("",queueName,null,body);
    }
}