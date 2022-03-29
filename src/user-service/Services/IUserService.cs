using core.ServerResponse;
using user_service.Models.Dtos.Requests;
using user_service.Models.Dtos.Responses;

namespace user_service.Services;

public interface IUserService
{
    Task<Response<IEnumerable<UserResponse>>> GetAll();
    Task<Response<UserResponse>> GetById(Guid id);
    Task<Response<UserResponse>> GetByEmail(string request);
    Task<Response<string>> Create(CreateUserRequest request);
    Task<Response<bool>> Update(UpdateUserRequest request);
    Task<Response<bool>> Delete(Guid id);
}