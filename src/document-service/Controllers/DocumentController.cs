using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Services;
using Microsoft.AspNetCore.Mvc;

namespace document_service.Controllers;

[ApiController]
[Route("api/[controller]s")]
public class DocumentController: ControllerBase
{
    private readonly IDocumentService _service;

    public DocumentController(IDocumentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        var documents = await _service.GetAll();
        return Ok(documents);
    }

    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var document = await _service.GetById(id);
        return Ok(document);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, string description)
    {
        var request = new CreateDocumentRequest {FormFile = file, LaterName = description};
        var result = await _service.Create(request);
        return Created("api/documents",result);
    }
        
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(IFormFile file,Guid id, string description)
    {
        var request = new UpdateDocumentRequest {FormFile = file, Description = description};
        var result = await _service.Update(id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _service.Delete(id);
        return Ok(result);
    }
}