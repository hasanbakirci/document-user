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
            Email = request.Email,
            Name = request.Name,
            Extension = request.Extension,
            Password = request.Password,
            Path = request.Path,
            Username = request.Username,
            DocumentId = Guid.Parse(request.DocumentId),
            UserId = Guid.Parse(request.UserId),
            DocumentCreatedAt = request.DocumentCreatedAt,
            DocumentUpdatedAt = request.DocumentUpdatedAt,
            UserCreatedAt = request.UserCreatedAt,
            UserUpdatedAt = request.UserUpdatedAt
        };
    }
}