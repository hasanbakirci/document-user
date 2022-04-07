using System.Diagnostics;
using System.Net;
using System.Text.Json;
using core.Exceptions;
using core.ServerResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace core.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ErrorDetails e)
        {
            await Handle(context, e);
        }
        catch (Exception e)
        {
            await Handle(context, e);
        }
    }

    private async Task Handle(HttpContext context, ErrorDetails exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = exception.StatusCode;
        await response.WriteAsync(exception.ToString());
    }
    
    private async Task Handle(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";
        response.StatusCode = 500;
        await response.WriteAsync(new ErrorDetails(500,5000,"Server Error").ToString());
    }

}