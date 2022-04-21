namespace core.Masstransit.Events;

public interface IUpdateDocumentEvent
{
    public Guid DocumentId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Extension { get; set; }
    public string Path { get; set; }
    public string MimeType { get; set; }
    public DateTime DocumentCreatedAt { get; set; }
    public DateTime DocumentUpdatedAt { get; set; }
    public Guid UserId { get; set; }
}