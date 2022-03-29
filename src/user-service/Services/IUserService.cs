using user_service.Models.Dtos.Requests;
using user_service.Models.Dtos.Responses;

namespace user_service.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponse>> GetAll();
    Task<UserResponse> GetById(Guid id);
    Task<UserResponse> GetByEmail(string request);
    Task<string> Create(CreateUserRequest request);
    Task<bool> Update(UpdateUserRequest request);
    Task<bool> Delete(Guid id);
}