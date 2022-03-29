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

    public async Task<IEnumerable<UserResponse>> GetAll()
    {
        var users = await _repository.GetAll();
        return users.ToUsersResponse();
    }

    public async Task<UserResponse> GetById(Guid id)
    {
        var user = await _repository.GetById(id);
        return user.ToUserResponse();
    }

    public async Task<UserResponse> GetByEmail(string request)
    {
        var user = await _repository.GetByEmail(request);
        return user.ToUserResponse();
    }

    public async Task<string> Create(CreateUserRequest request)
    {
        var result = GetByEmail(request.Email);
        if (result.Result.Email != request.Email)
        {
           return await _repository.Create(request.ToUser()); 
        }

        return "error";
    }

    public async Task<bool> Update(UpdateUserRequest request)
    {
        var result = GetByEmail(request.Email);
        if (result.Result.Email != request.Email)
        {
           return await _repository.Update(request.ToUser()); 
        }
        return false;
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _repository.Delete(id);
    }
    
}