﻿using core.Masstransit.Events;
using MassTransit;
using worker_service.Models.Requests;
using worker_service.Services;

namespace worker_service.Consumers;

public class CreateDocumentEventConsumer : IConsumer<IRequestDocumentEvent>
{
    private readonly ILoggerService _loggerService;
    
    public CreateDocumentEventConsumer(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }
    
    public Task Consume(ConsumeContext<IRequestDocumentEvent> context)
    {
        //Console.WriteLine($"#CCCCC# {context.Message.Description} isimli dosya create event ile iletildi.");
        var result = _loggerService.Create(new CreateLogRequest
         {
             Description = context.Message.Description,
             Extension = context.Message.Extension,
             Name = context.Message.Name,
             Path = context.Message.Path,
             DocumentId = context.Message.DocumentId.ToString(),
             MimeType = context.Message.MimeType,
             UserId = context.Message.UserId.ToString(),
             DocumentCreatedAt = context.Message.DocumentCreatedAt,
             DocumentUpdatedAt = context.Message.DocumentUpdatedAt
         });
        Console.WriteLine($"--create-- {result}  ----");
        return Task.CompletedTask;
    }
}

public class CreateDocumentEventConsumerDefinition : ConsumerDefinition<CreateDocumentEventConsumer>{
    public CreateDocumentEventConsumerDefinition()
    {
        EndpointName = "create-request-queue";
        ConcurrentMessageLimit = 10;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<CreateDocumentEventConsumer> consumerConfigurator)
    {
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100,200,500,800,1000));
        endpointConfigurator.UseInMemoryOutbox();
    }
}