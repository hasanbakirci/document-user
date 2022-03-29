using core.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace core.Api;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    [NonAction]
    protected IActionResult ApiResponse(Response response) => StatusCode(response.StatusCode, response);

    [NonAction]
    protected IActionResult ApiResponse<T>(Response<T> response) => StatusCode(response.StatusCode, response);

}