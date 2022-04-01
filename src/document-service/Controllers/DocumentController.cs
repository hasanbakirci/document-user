using core.Api;
using document_service.ActionFilters;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace document_service.Controllers;

[TokenFilter]
[ApiController]
[Route("api/[controller]s")]
public class DocumentController: ApiController
{
    private readonly IDocumentService _service;

    public DocumentController(IDocumentService service)
    {
        _service = service;
    }
    
    [RoleFilter("admin","user")]
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    { 
        //this.Request.
        var documents = await _service.GetAll();
        return ApiResponse(documents);
    }

    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var document = await _service.GetById(id);
        return ApiResponse(document);
    }
    
    [RoleFilter("user")]
    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, string description)
    {
        var request = new CreateDocumentRequest {FormFile = file, LaterName = description};
        var result = await _service.Create(request);
        return ApiResponse(result);
    }
        
    [HttpPut("{id}/{description}")]
    public async Task<IActionResult> Update(IFormFile file,string id, string description)
    {
        var request = new UpdateDocumentRequest {FormFile = file, Description = description};
        var result = await _service.Update(Guid.Parse(id),request);
        return ApiResponse(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.Delete(id);
        return ApiResponse(result);
    }
}