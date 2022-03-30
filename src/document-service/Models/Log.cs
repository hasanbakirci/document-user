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
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public DateTime UserCreatedAt { get; set; }
    public DateTime UserUpdatedAt { get; set; }
}