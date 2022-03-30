namespace user_service.Helper;

public interface IJwtHelper
{
    AccessTokenResponse GenereteJwtToken(Guid id, string role);
    TokenHandlerResponse ValidateJwtToken(string token);
}