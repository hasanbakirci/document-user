using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using core.Validation;
using document_service.Extensions;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Requests.RequestValidations;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.UserOperations.Commands;

public class CreateUserCommand : IRequest<Response<string>>
{
    public CreateUserRequest CreateUserRequest { get; set; }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<string>>
{
    private readonly IUserRepository _repository;

    public CreateUserCommandHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        ValidationTool.Validate(new CreateUserRequestValidator(),request.CreateUserRequest);
        
        var user = await _repository.GetByEmail(request.CreateUserRequest.Email);
        if (user is not null)
        {
            throw new DuplicateKeyException(request.CreateUserRequest.Email);
        }

        var result = await _repository.InsertOne(request.CreateUserRequest.ToUser());
        return new SuccessResponse<string>(result.Id.ToString()); 
    }
}