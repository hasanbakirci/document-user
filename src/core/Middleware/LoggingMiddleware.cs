using System.Diagnostics;
using core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace core.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
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
        catch (ErrorDetails e)
        {
            _logger.LogInformation(e.ToString());
            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            throw;
        }
    }
}