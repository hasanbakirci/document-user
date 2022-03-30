namespace user_service.Models.Dtos.Requests;

public class LoginRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}