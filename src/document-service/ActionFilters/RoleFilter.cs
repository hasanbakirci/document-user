using core.ServerResponse;
using document_service.Clients.UserClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace document_service.ActionFilters;

public class RoleFilterAttribute : TypeFilterAttribute
{
    public RoleFilterAttribute(params string[] roles) : base(typeof(RoleFilter))
    {
        Arguments = new object[] {roles};
    }
}

public class RoleFilter : IAsyncActionFilter
{
    private readonly string[] _roles;
    private readonly IUserClient _userClient;
    public RoleFilter(string[] roles, IUserClient userClient)
    {
        _roles = roles;
        _userClient = userClient;
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token == null)
        {
            //throw new UnauthorizedAccessException();
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Token not found."));
            return;
        }
        var tokenHandlerResponse = _userClient.TokenValidate(token);
        if (!tokenHandlerResponse.Result.Success || !_roles.ToList().Contains(tokenHandlerResponse.Result.Data.Role))
        {
            //throw new UnauthorizedAccessException();
            context.Result = new UnauthorizedObjectResult(new ErrorResponse(ResponseStatus.UnAuthorized,"Invalid token"));
            return;
        }
        await next();
    }
}