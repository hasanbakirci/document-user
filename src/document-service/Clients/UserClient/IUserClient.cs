using core.ServerResponse;

namespace document_service.Clients.UserClient;

public interface IUserClient
{
    Task<Response<TokenHandlerResponse>> TokenValidate(string token);
    Task<Response<UserHandlerResponse>> SearchUser(Guid id);
}