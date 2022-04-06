namespace core.Exceptions;

public class BadRequest : ErrorDetails
{
    public BadRequest(int errorCode,string errorMessage)
    {
        ErrorCode = errorCode;
        StatusCode = 400;
        ErrorMessage = errorMessage;
    }
}