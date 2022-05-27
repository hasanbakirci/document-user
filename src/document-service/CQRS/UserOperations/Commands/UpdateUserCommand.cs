using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using core.Validation;
using document_service.Extensions;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Requests.RequestValidations;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.UserOperations.Commands;

public class UpdateUserCommand : IRequest<Response<bool>>
{
    public Guid Id { get; set; }
    public UpdateUserRequest UpdateUserRequest { get; set; }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<bool>>
{
    private readonly IUserRepository _repository;

    public UpdateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        ValidationTool.Validate(new UpdateUserRequestValidator(), request.UpdateUserRequest);

        var result = await _repository.UpdateUser(request.Id, request.UpdateUserRequest.ToUser());
        
        if (result)
        {
            return new SuccessResponse<bool>(true);
        }
        
        throw new DocumentNotFound(request.Id);
    }
}