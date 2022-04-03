namespace worker_service.Models.Requests;

public class CreateLogRequest
{
    public string DocumentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Extension { get; set; }
    public string Path { get; set; }
    public DateTime DocumentCreatedAt { get; set; }
    public DateTime DocumentUpdatedAt { get; set; }
    public string UserId { get; set; }

}