using core.ServerResponse;
using document_service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace document_service.ActionFilters;

public class TokenFilterAttribute : TypeFilterAttribute
{
    public TokenFilterAttribute() : base(typeof(TokenFilter))//
    {
        //Arguments = new object[] {roles};
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
        //throw new NotImplementedException();
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            //throw new UnauthorizedAccessException();
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Token not found."));
            return;
        }

        var tokenHandlerResponse = _userService.ValidateToken(token);
        if (!tokenHandlerResponse.Success)
        {
            //throw new UnauthorizedAccessException();
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid token"));
            return;
        }

        await next();
    }
}