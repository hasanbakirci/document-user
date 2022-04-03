using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;

namespace document_service.Extensions;

public static class ConverterExtensions
{
    public static DocumentResponse ToDocumentResponse(this Document document)
    {
        return new DocumentResponse
        {
            Id = document.Id,
            Name = document.Name,
            Description = document.Description,
            Extension = document.Extension,
            Path = document.Path,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
    }

    public static IEnumerable<DocumentResponse> ToDocumentsResponse(this IEnumerable<Document> documents)
    {
        List<DocumentResponse> documentResponses = new List<DocumentResponse>();
        documents.ToList().ForEach(
            d => documentResponses.Add(
                new DocumentResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Extension = d.Extension,
                    Path = d.Path,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                }));
        return documentResponses;
    }
    
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            Role = user.Role,
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
            Role = u.Role,
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
            Email = request.Email,
            Role = request.Role
        };
    }
    
    public static User ToUser(this UpdateUserRequest request)
    {
        return new User
        {
            Username = request.Username,
            Password = request.Password,
            Role = request.Role
        };
    }

    public static Log CreateLog(Document document, string userId)
    {
        return new Log
        {
            DocumentId = document.Id,
            Name = document.Name,
            Description = document.Description,
            Extension = document.Extension,
            Path = document.Path,
            DocumentCreatedAt = document.CreatedAt,
            DocumentUpdatedAt = document.UpdatedAt,
            UserId = Guid.Parse(userId)
        };
    }
}