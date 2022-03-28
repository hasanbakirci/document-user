namespace document_service.Models.Dtos.Requests;

public class UpdateDocumentRequest
{
    public IFormFile FormFile { get; set; }
    public string? Description { get; set; }
}