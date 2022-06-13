using core.Api;
using document_service.ActionFilters;
using document_service.CQRS.DocumentOperations.Commands;
using document_service.CQRS.DocumentOperations.Queries;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace document_service.Controllers;

[TokenFilter]
[ApiController]
[Route("api/[controller]s")]
public class DocumentController: ApiController
{
    private readonly IDocumentService _service;
    private readonly IMediator _mediator;

    public DocumentController(IDocumentService service, IMediator mediator)
    {
        _service = service;
        _mediator = mediator;
    }
    
    [RoleFilter("admin","write","user")]
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        // var documents = await _service.GetAll();
        //return ApiResponse(documents);
        return ApiResponse(await _mediator.Send(new GetDocumentsQuery()));
        
    }

    [HttpGet("Search/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // var document = await _service.GetById(id);
        // return ApiResponse(document);
        return ApiResponse(await _mediator.Send(new GetDocumentByIdQuery(){Id = id}));
    }
    
    [RoleFilter("admin","write")]
    [HttpPost]
    public async Task<IActionResult> Create(IFormFile file, string description)
    {
        //var request = new CreateDocumentRequest {FormFile = file, Description = description};
        // var result = await _service.Create(TokenHandler(Request),request);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new CreateDocumentCommand(){FormFile = file, Description = description,Token = TokenHandler(Request)}));
    }
    
    [RoleFilter("admin")]
    [HttpPut("{id}/{description}")]
    public async Task<IActionResult> Update(IFormFile file,string id, string description)
    {
        //var request = new UpdateDocumentRequest {FormFile = file, Description = description};
        // var result = await _service.Update(TokenHandler(Request),Guid.Parse(id),request);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new UpdateDocumentCommand(){Id = Guid.Parse(id),Token = TokenHandler(Request),FormFile= file,Description = description}));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        // var result = await _service.Delete(id);
        // return ApiResponse(result);
        return ApiResponse(await _mediator.Send(new DeleteDocumentCommand(){Id = id}));
    }

    private string TokenHandler(HttpRequest request)
    {
        var token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        return token;
    }
}