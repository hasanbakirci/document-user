using document_service.Models.Dtos.Responses;

namespace document_service.Helpers.JWT;

public interface IJwtHelper
{
    AccessTokenResponse GenereteJwtToken(Guid id, string role);
    TokenHandlerResponse ValidateJwtToken(string token);
}