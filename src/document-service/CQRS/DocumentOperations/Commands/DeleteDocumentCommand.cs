using core.ServerResponse;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Commands;

public class DeleteDocumentCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}