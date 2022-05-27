using core.ServerResponse;
using document_service.Extensions;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;
using MediatR;

namespace document_service.CQRS.UserOperations.Queries;

public class GetUserByIdQuery : IRequest<Response<UserResponse>>
{
    public Guid Id { get; set; }
}

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Response<UserResponse>>
{
    private readonly IUserRepository _repository;

    public GetUserByIdQueryHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(request.Id);
        return new SuccessResponse<UserResponse>(user.ToUserResponse());
    }
}