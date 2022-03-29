using System.Net;
using System.Text.Json;
using core.ServerResponse;
using Microsoft.AspNetCore.Http;

namespace core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context){
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var resp = new ErrorResponse(ResponseStatus.Internal,e.Message);
            var result = JsonSerializer.Serialize(resp);

            await response.WriteAsync(result);
        }
    }

}