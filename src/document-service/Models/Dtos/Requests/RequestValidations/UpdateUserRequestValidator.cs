using FluentValidation;

namespace document_service.Models.Dtos.Requests.RequestValidations;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(r => r.Username).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(r => r.Password).NotEmpty().MinimumLength(3).MaximumLength(20);
        RuleFor(r => r.Role).Must(ChooseRole).WithMessage("Only admin, write or user values can be entered").NotEmpty();
    }

    
    private bool ChooseRole(string arg)
    {
        string[] roleList = {"admin","write","user"};
        var result = Array.Exists(roleList, arg.Contains);
        return result;
    }
}