namespace document_service.Models.Dtos.Requests;

public class LogRequest
{
    public Guid DocumentId { get; set; }
    public Guid UserId { get; set; }
}