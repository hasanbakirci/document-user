using core.ServerResponse;
using user_service.Extensions;
using user_service.Models.Dtos.Requests;
using user_service.Models.Dtos.Responses;
using user_service.Repositories;

namespace user_service.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<UserResponse>>> GetAll()
    {
        var users = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<UserResponse>>(users.ToUsersResponse());
    }

    public async Task<Response<UserResponse>> GetById(Guid id)
    {
        var user = await _repository.GetById(id);
        if (user is not null)
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);

    }

    public async Task<Response<UserResponse>> GetByEmail(string request)
    {
        var user = await _repository.GetByEmail(request);
        if (user is not null)
            return new SuccessResponse<UserResponse>(user.ToUserResponse());
        return new ErrorResponse<UserResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
    }

    public async Task<Response<string>> Create(CreateUserRequest request)
    {
        var result = GetByEmail(request.Email);
        if (result.Result.Success)
        {
            return new ErrorResponse<string>(ResponseStatus.BadRequest,default, ResultMessage.Error);
        }
        return new SuccessResponse<string>(await _repository.Create(request.ToUser())); 
    }

    public async Task<Response<bool>> Update(UpdateUserRequest request)
    {
        var user = GetById(request.Id);
        if (!user.Result.Success)
        {
            return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
        }
        var result = GetByEmail(request.Email);
        if (result.Result.Success)
        {
            return new ErrorResponse<bool>(ResponseStatus.BadRequest,default, ResultMessage.Error);
        }

        var newUser = request.ToUser();
        newUser.CreatedAt = user.Result.Data.CreatedAt;
        return new SuccessResponse<bool>(await _repository.Update(newUser));
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        var user = GetById(id);
        if (!user.Result.Success)
        {
            return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundUser);
        }
        var result = await _repository.Delete(id);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }

        return new ErrorResponse<bool>(result);
    }
    
}