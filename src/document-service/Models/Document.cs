
namespace document_service.Models;

public class Document
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Extension { get; set; }
    public string Path { get; set; }
    public string MimeType { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}