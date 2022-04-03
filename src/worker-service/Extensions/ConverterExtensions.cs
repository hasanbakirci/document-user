using worker_service.Models;
using worker_service.Models.Requests;

namespace worker_service.Extensions;

public static class ConverterExtensions
{
    public static Log ToLog(this CreateLogRequest request)
    {
        return new Log
        {
            Description = request.Description,
            Name = request.Name,
            Extension = request.Extension,
            Path = request.Path,
            DocumentId = request.DocumentId,
            UserId = request.UserId,
            DocumentCreatedAt = request.DocumentCreatedAt,
            DocumentUpdatedAt = request.DocumentUpdatedAt,
        };
    }
}