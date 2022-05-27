using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.UserOperations.Commands;

public class DeleteUserCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Response<bool>>
{
    private readonly IUserRepository _repository;

    public DeleteUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteById(request.Id);
        if (result)
        {
            return new SuccessResponse<bool>(true);
        }
        
        throw new DocumentNotFound(request.Id);
    }
}