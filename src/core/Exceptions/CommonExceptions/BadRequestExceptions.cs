namespace core.Exceptions.CommonExceptions;

public class ValidateException : ErrorDetails
{
    public ValidateException(string errorMessage) : base(400,4001, errorMessage)
    {
    }
    
}

public class MimeTypeException : ErrorDetails
{
    public MimeTypeException(string errorMessage) : base(400,4002, "wrong file format, only .txt or .word can be uploaded")
    {
    }
}

public class DuplicateKeyException : ErrorDetails
{
    public DuplicateKeyException(string value) : base(400,4003, $"{value} was already exists")
    {
    }
}

public class LoginException : ErrorDetails
{
    public LoginException() : base(400,4004, $"Your email or password is incorrect")
    {
    }
}