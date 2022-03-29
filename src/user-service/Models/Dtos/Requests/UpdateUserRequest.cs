namespace user_service.Models.Dtos.Requests;

public class UpdateUserRequest
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}