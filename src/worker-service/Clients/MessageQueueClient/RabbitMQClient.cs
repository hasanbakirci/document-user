using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace worker_service.Clients.MessageQueueClient;

public class RabbitMQClient :IMessageQueueClient
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
        _channel.QueueDeclare(queue: RabbitMQHelper.LoggerQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
    }
    public void Subscribe<T>(string queueName, Action<T> callback)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var messageObject = JsonConvert.DeserializeObject<T>(message);
            //try
            // {
                callback.Invoke(messageObject);
            // }
            // catch (Exception e)
            // {
            //     _channel.BasicReject(ea.DeliveryTag, true);
            //     throw;
            // }
            //
            // _channel.BasicAck(ea.DeliveryTag, true);
        };

        _channel.BasicConsume(queue: queueName,autoAck:true,consumer:consumer);
    }
}