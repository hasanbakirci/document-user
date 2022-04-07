using core.Exceptions.CommonExceptions;
using core.ServerResponse;
using document_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace document_service.ActionFilters;

public class TokenFilterAttribute : TypeFilterAttribute
{
    public TokenFilterAttribute() : base(typeof(TokenFilter))
    {

    }
}
public class TokenFilter : IAsyncActionFilter
{
    private readonly IUserService _userService;

    public TokenFilter(IUserService userService)
    {
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            //throw new UnauthorizedAccessException();
            //context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Token not found."));
            //return;
            throw new NullTokenException();
        }

        var tokenHandlerResponse = _userService.ValidateToken(token);
        if (!tokenHandlerResponse.Success)
        {
            //throw new UnauthorizedAccessException();
            //context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid token"));
            //return;
            throw new InvalidTokenException();
        }

        await next();
    }
}