namespace document_service.Models.Dtos.Requests;

public class CreateUserRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
}