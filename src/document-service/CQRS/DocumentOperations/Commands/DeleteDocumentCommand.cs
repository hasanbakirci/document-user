using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Commands;

public class DeleteDocumentCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Response<bool>>
{
    private readonly IDocumentRepository _repository;

    public DeleteDocumentCommandHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteById(request.Id);
        if (result)
        {
            return new SuccessResponse<bool>(true);
        }
        
        throw new DocumentNotFound(request.Id);
    }
}