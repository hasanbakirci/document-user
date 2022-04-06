namespace core.Exceptions;

public class NotFound : ErrorDetails
{
    public NotFound(int errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        StatusCode = 404;
        ErrorMessage = errorMessage;
    }
}