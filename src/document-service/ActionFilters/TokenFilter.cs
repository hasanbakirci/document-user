using core.ServerResponse;
using document_service.Clients.UserClient;
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
    private readonly IUserClient _userClient;

    public TokenFilter(IUserClient userClient)
    {
        _userClient = userClient;
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
        var tokenHandlerResponse = _userClient.TokenValidate(token);
        if (!tokenHandlerResponse.Result.Success)
        {
            //throw new UnauthorizedAccessException();
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid token"));
            return;
        }

        await next();
    }
}