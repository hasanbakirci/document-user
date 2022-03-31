using Newtonsoft.Json;
using worker_service.Clients.MessageQueueClient;
using worker_service.Models.Requests;
using worker_service.Services;

namespace worker_service;

public class Worker : BackgroundService
{
    private readonly IMessageQueueClient _messageQueueClient;
    private readonly ILoggerService _loggerService;

    public Worker(ILoggerService loggerService, IMessageQueueClient messageQueueClient)
    {
        _loggerService = loggerService;
        _messageQueueClient = messageQueueClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _messageQueueClient.Subscribe(RabbitMQHelper.LoggerQueue, new Action<CreateLogRequest>(l =>
            {
                _loggerService.Create(l);
                //Console.WriteLine(JsonConvert.SerializeObject(l));
            }));
            await Task.Delay(1000, stoppingToken);
        }
    }
}