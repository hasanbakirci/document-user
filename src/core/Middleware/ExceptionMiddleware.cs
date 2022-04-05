using System.Diagnostics;
using System.Net;
using System.Text.Json;
using core.ServerResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context){
        var watch = Stopwatch.StartNew();
        try
        {
            string message = "[Request] HTTP "+ context.Request.Method + " - "+ context.Request.Path;
            _logger.LogInformation(message);
            
            await _next(context);
            
            watch.Stop();
            
            message = "[Response] HTTP "+ context.Request.Method + " - "
                      + context.Request.Path + " responded " + context.Response.StatusCode + 
                      " in "+watch.Elapsed.TotalMilliseconds +"ms";
            _logger.LogInformation(message);
        }
        catch (Exception e)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var resp = new ErrorResponse(ResponseStatus.Internal,e.Message);
            var result = JsonSerializer.Serialize(resp);
            
            _logger.LogError(result);
            
            await response.WriteAsync(result);
        }
    }

}