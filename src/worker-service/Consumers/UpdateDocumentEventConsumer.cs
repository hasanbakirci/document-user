using core.Masstransit.Events;
using MassTransit;
using worker_service.Models;
using worker_service.Models.Requests;
using worker_service.Services;

namespace worker_service.Consumers;

public class UpdateDocumentEventConsumer : IConsumer<IRequestDocumentEvent>
{
    private readonly IElasticSearchService _elasticSearchService;

    public UpdateDocumentEventConsumer(IElasticSearchService elasticSearchService)
    {
        _elasticSearchService = elasticSearchService;
    }

    public Task Consume(ConsumeContext<IRequestDocumentEvent> context)
    {
        //Console.WriteLine($"#UUUUU# {context.Message.Description} isimli dosya update event ile iletildi.");
        
        var result =_elasticSearchService.InsertLog(new Log
        {
            Id = Guid.NewGuid(),
            Description = context.Message.Description,
            Extension = context.Message.Extension,
            Name = context.Message.Name,
            Path = context.Message.Path,
            DocumentId = context.Message.DocumentId.ToString(),
            MimeType = context.Message.MimeType,
            UserId = context.Message.UserId.ToString(),
            DocumentCreatedAt = context.Message.DocumentCreatedAt,
            DocumentUpdatedAt = context.Message.DocumentUpdatedAt,
            Status = "Update"
        });
        Console.WriteLine($"--update-- {result} ----");
        return Task.CompletedTask;
    }
}