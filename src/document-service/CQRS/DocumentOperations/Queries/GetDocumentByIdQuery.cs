using core.ServerResponse;
using document_service.Models.Dtos.Responses;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Queries;

public class GetDocumentByIdQuery : IRequest<Response<DocumentResponse>>
{
    public Guid Id { get; set; }
}