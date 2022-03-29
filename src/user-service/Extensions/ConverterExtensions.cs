using user_service.Models;
using user_service.Models.Dtos.Requests;
using user_service.Models.Dtos.Responses;

namespace user_service.Extensions;

public static class ConverterExtensions
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static IEnumerable<UserResponse> ToUsersResponse(this IEnumerable<User> users)
    {
        List<UserResponse> userResponses = new List<UserResponse>();
        users.ToList().ForEach(u => userResponses.Add(new UserResponse
        {
            Id = u.Id,
            Username = u.Username,
            Password = u.Password,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt
        }));
        return userResponses;
    }

    public static User ToUser(this CreateUserRequest request)
    {
        return new User
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email
        };
    }
    
    public static User ToUser(this UpdateUserRequest request)
    {
        return new User
        {
            Id = request.Id,
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
        };
    }
}