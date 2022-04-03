using core.Api;
using document_service.Models.Dtos.Requests;
using document_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace document_service.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ApiController
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }
        
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        var users = await _service.GetAll();
        return ApiResponse(users);
    }
        
    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetById(id);
        return ApiResponse(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var result = await _service.Create(request);
        return ApiResponse(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id,UpdateUserRequest request)
    { 
        var result = await _service.Update(id,request);
        return ApiResponse(result);
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.Delete(id);
        return ApiResponse(result);
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