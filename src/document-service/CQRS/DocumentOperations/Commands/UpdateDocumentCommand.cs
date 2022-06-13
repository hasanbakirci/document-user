using document_service.Models.Dtos.Requests;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Commands;

public class UpdateDocumentCommand : IRequest<core.ServerResponse.Response<bool>>
{
    public Guid Id { get; set; }
    public IFormFile FormFile { get; set; }
    public string Description { get; set; }
    public string Token { get; set; }
}