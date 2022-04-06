using Newtonsoft.Json;

namespace core.Exceptions;

public class ErrorDetails : Exception
{
    public int StatusCode { get; set; }
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; } = "error";

    public ErrorDetails()
    {
        
    }
    [JsonConstructor]
    public ErrorDetails(int statusCode,int errorCode,string errorMessage)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(new
        {
            statusCode = StatusCode,
            errorCode = ErrorCode,
            errorMessage = ErrorMessage
        });
    }
}