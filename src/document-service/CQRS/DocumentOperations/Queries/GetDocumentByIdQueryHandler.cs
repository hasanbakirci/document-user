using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using document_service.Extensions;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Queries;

public class GetDocumentByIdQueryHandler : IRequestHandler<GetDocumentByIdQuery, Response<DocumentResponse>>
{
    private IDocumentRepository _repository;

    public GetDocumentByIdQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<DocumentResponse>> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
    {
        var document = await _repository.GetById(request.Id);
        if (document is not null)
        {
            return new SuccessResponse<DocumentResponse>(document.ToDocumentResponse());
        }
        
        throw new DocumentNotFound(request.Id);
    }
}