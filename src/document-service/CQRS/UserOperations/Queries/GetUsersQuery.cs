using core.ServerResponse;
using document_service.Extensions;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.UserOperations.Queries;

public class GetUsersQuery :IRequest<Response<IEnumerable<UserResponse>>>
{
    
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Response<IEnumerable<UserResponse>>>
{
    private readonly IUserRepository _repository;

    public GetUsersQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<UserResponse>>(users.ToUsersResponse());
    }
}