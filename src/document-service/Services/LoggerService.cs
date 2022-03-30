using document_service.Clients.UserClient;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using Newtonsoft.Json;

namespace document_service.Services;

public class LoggerService :ILoggerService
{
    private readonly IDocumentService _documentService;
    private readonly IUserClient _userClient;

    public LoggerService(IDocumentService documentService, IUserClient userClient)
    {
        _documentService = documentService;
        _userClient = userClient;
    }

    public void SendLog(LogRequest request)
    {
        var userResponse = _userClient.SearchUser(request.UserId);
        var documentResponse = _documentService.GetById(request.DocumentId);
        var user = userResponse.Result.Data;
        var document = documentResponse.Result.Data;
        var log = new Log
        {
            DocumentId = document.Id,
            Name = document.Name,
            Description = document.Description,
            Extension = document.Extension,
            Path = document.Path,
            DocumentCreatedAt = document.CreatedAt,
            DocumentUpdatedAt = document.UpdatedAt,
            UserId = user.Id,
            Email = user.Email,
            Username = user.Username,
            Password = user.Password,
            UserCreatedAt = user.CreatedAt,
            UserUpdatedAt = user.UpdatedAt
        };
        Console.WriteLine(JsonConvert.SerializeObject(log));
    }
}