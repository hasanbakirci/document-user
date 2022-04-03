using document_service.Models.Dtos.Responses;

namespace document_service.Models;

public class Log
{
    public Guid DocumentId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Extension { get; set; }
    public string? Path { get; set; }
    public DateTime DocumentCreatedAt { get; set; }
    public DateTime DocumentUpdatedAt { get; set; }
    public Guid UserId { get; set; }
}