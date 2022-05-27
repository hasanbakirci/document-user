using core.ServerResponse;
using document_service.Extensions;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Queries;

public class GetDocumentsQuery : IRequest<Response<IEnumerable<DocumentResponse>>>
{
    
}

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, Response<IEnumerable<DocumentResponse>>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentsQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<DocumentResponse>>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        var documents = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<DocumentResponse>>(documents.ToDocumentsResponse());
    }
}
