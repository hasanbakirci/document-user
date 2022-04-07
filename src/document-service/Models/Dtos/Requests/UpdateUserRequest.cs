namespace document_service.Models.Dtos.Requests;

public class UpdateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}