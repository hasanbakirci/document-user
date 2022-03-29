using core.Api;
using Microsoft.AspNetCore.Mvc;
using user_service.Models.Dtos.Requests;
using user_service.Services;

namespace user_service.Controllers;

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
    public async Task<IActionResult> Update(UpdateUserRequest request)
    { 
        var result = await _service.Update(request);
        return ApiResponse(result);
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.Delete(id);
        return ApiResponse(result);
    }
    }
