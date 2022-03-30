namespace document_service.Clients.UserClient;

public class UserClientSettings
{
    public static string baseUrl = "https://localhost:7250/api/Users/";
    public static string ValidateTokenUrl = baseUrl+"ValidateToken";
    public static string SearchUserUrl = baseUrl+"Search/";
}