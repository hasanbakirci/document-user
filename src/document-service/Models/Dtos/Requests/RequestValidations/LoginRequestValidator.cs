using FluentValidation;

namespace document_service.Models.Dtos.Requests.RequestValidations;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(r => r.Password).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(r => r.Email).NotEmpty().EmailAddress();
    }
}