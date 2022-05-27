using core.Api;
using document_service.ActionFilters;
using document_service.CQRS.UserOperations.Commands;
using document_service.CQRS.UserOperations.Queries;
using document_service.Models.Dtos.Requests;
using document_service.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace document_service.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ApiController
{
    private readonly IUserService _service;
    private readonly IMediator _mediator;

    public UserController(IUserService service, IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }
        
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        // var users = await _service.GetAll();
        // return ApiResponse(users);
        return ApiResponse(await _mediator.Send(new GetUsersQuery()));
    }
        
    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // var user = await _service.GetById(id);
        // return ApiResponse(user);
        return ApiResponse(await _mediator.Send(new GetUserByIdQuery(){Id = id}));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        // var result = await _service.Create(request);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new CreateUserCommand(){CreateUserRequest = request}));
    }
    
    [RoleFilter("admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,UpdateUserRequest request)
    { 
        // var result = await _service.Update(id,request);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new UpdateUserCommand(){Id = id,UpdateUserRequest = request}));
    }
    
    [RoleFilter("admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // var result = await _service.Delete(id);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new DeleteUserCommand(){Id = id}));
    }
    
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _service.Login(request);
        return ApiResponse(result);
    }

    [HttpPost("ValidateToken")]
    public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
    {
        var result = _service.ValidateToken(request.token);
        return ApiResponse(result);
    }
}