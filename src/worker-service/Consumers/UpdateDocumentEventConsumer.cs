using core.Masstransit.Events;
using MassTransit;
using worker_service.Models.Requests;
using worker_service.Services;

namespace worker_service.Consumers;

public class UpdateDocumentEventConsumer : IConsumer<IUpdateDocumentEvent>
{
    private readonly ILoggerService _loggerService;

    public UpdateDocumentEventConsumer(ILoggerService loggerService)
    {
        _loggerService = loggerService;
    }

    public Task Consume(ConsumeContext<IUpdateDocumentEvent> context)
    {
        //Console.WriteLine($"#UUUUU# {context.Message.Description} isimli dosya update event ile iletildi.");
        
        var result =_loggerService.Create(new CreateLogRequest
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
        Console.WriteLine($"---- {result} ----");
        return Task.CompletedTask;
    }
}