namespace core.Exceptions;

public class ServerError : ErrorDetails
{
    public ServerError(string errorMessage)
    {
        ErrorCode = 5000;
        StatusCode = 500;
        ErrorMessage = errorMessage;
    }
}