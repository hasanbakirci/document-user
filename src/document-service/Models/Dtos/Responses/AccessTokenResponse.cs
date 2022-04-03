namespace document_service.Models.Dtos.Responses;

public class AccessTokenResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; } 
}