namespace core.ServerResponse;

public class Response
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public Response()
    {
            
    }

    public Response(ResponseStatus status, string message)
    {
        StatusCode = (int)status;
        Success = StatusCode < 400;
        Message = message;
    }

}

public class Response<T> : Response
{
    public T Data { get; set; }
    public Response(ResponseStatus status, string message) : base(status,message)
    {
            
    }
}
