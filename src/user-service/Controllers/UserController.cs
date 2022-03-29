using Microsoft.AspNetCore.Mvc;
using user_service.Models.Dtos.Requests;
using user_service.Services;

namespace user_service.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class UserController : ControllerBase
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
        return Ok(users);
    }
        
    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetById(id);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var result = await _service.Create(request);
        return Created("api/users", result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(UpdateUserRequest request)
    { 
        var result = await _service.Update(request);
        return Ok(result);
    }
        
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.Delete(id);
        return Ok(result);
    }
    }
