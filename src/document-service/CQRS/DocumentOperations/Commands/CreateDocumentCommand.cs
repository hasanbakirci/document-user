using document_service.Models.Dtos.Requests;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Commands;

public class CreateDocumentCommand : IRequest<core.ServerResponse.Response<string>>
{
    public IFormFile FormFile { get; set; }
    public string Description { get; set; }
    public string Token { get; set; }
}