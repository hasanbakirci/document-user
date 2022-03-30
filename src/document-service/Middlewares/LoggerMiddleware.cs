using document_service.Clients.UserClient;
using document_service.Models.Dtos.Requests;
using document_service.Services;

namespace document_service.Middlewares;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IUserClient _userClient;
    private readonly ILoggerService _loggerService;

    public LoggerMiddleware(IUserClient userClient, RequestDelegate next, ILoggerService loggerService)
    {
        _userClient = userClient;
        _next = next;
        _loggerService = loggerService;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Method.Contains("POST") || context.Request.Method.Contains("PUT"))
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                throw new UnauthorizedAccessException();
            }
            var tokenHandlerResponse = _userClient.TokenValidate(token);
            if (!tokenHandlerResponse.Result.Success)
            {
                throw new UnauthorizedAccessException();
            }


            Stream originalBody = context.Response.Body;
            try {
                using (var memStream = new MemoryStream()) {
                    context.Response.Body = memStream;
                    
                    await _next(context);
                    
                    memStream.Position = 0;
                    string responseBody = new StreamReader(memStream).ReadToEnd();
                    _loggerService.SendLog(new LogRequest
                    {
                        DocumentId = Guid.Parse(responseBody.Split('"')[3]),
                        UserId = Guid.Parse(tokenHandlerResponse.Result.Data.Id)
                    });
                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                }

            } finally {
                context.Response.Body = originalBody;
            }
        }
        else
        {
            await _next(context);
        }
    }
    
}