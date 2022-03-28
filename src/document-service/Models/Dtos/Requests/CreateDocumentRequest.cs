namespace document_service.Models.Dtos.Requests;

public class CreateDocumentRequest
{
    public IFormFile FormFile { get; set; }
    public string? LaterName { get; set; }
}