namespace core.Exceptions.CommonExceptions;

public class ValidateException : BadRequest
{
    public ValidateException(string errorMessage) : base(4001, errorMessage)
    {
    }
    
}

public class MimeTypeException : BadRequest
{
    public MimeTypeException(string errorMessage) : base(4002, "wrong file format, only .txt or .pdf can be uploaded")
    {
    }
}

public class DuplicateKeyException : BadRequest
{
    public DuplicateKeyException(string value) : base(4003, $"{value} was already exists")
    {
    }
}