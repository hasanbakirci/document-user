using core.Exceptions.CommonExceptions;
using FluentValidation;

namespace core.Validation;

public class ValidationTool
{
    public static void Validate(IValidator validator, object entity){
        var context = new ValidationContext<object>(entity);
        var result = validator.Validate(context);
        string message = "";
        
        if(!result.IsValid){
            foreach (var failure in result.Errors)
            {
                message +="Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage +" ";
            }
            //throw new ValidationException(result.Errors);
            throw new ValidateException(message);
        }
    }
}