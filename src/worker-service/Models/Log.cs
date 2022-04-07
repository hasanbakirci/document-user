namespace worker_service.Models;

public class Log
{
    public Guid id { get; set; }
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Extension { get; set; }
    public string Path { get; set; }
    public string MimeType  { get; set; }
    public DateTime DocumentCreatedAt { get; set; }
    public DateTime DocumentUpdatedAt { get; set; }
    public string UserId { get; set; }

}