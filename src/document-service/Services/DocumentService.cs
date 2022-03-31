using core.ServerResponse;
using document_service.Extensions;
using document_service.Helpers;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;

namespace document_service.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;

    public DocumentService(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Response<IEnumerable<DocumentResponse>>> GetAll()
    {
        var documents = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<DocumentResponse>>(documents.ToDocumentsResponse());
    }

    public async Task<Response<DocumentResponse>> GetById(Guid id)
    {
        var document = await _repository.GetById(id);
        if (document is not null)
        {
            return new SuccessResponse<DocumentResponse>(document.ToDocumentResponse());
        }

        return new ErrorResponse<DocumentResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
    }

    public async Task<Response<string>> Create(CreateDocumentRequest request)
    {
        var fileResponse = await FileHelper.Add(request.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.LaterName
        };
        var result =await _repository.Create(document);
        if (result is not null)
        {
            return new SuccessResponse<string>(result);
        }

        return new ErrorResponse<string>(ResponseStatus.BadRequest,default,ResultMessage.Error);


    }

    public async Task<Response<bool>> Update(Guid id, UpdateDocumentRequest request)
    {
        var document = GetById(id);
        if (!document.Result.Success)
        {
            return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
        }
        var fileResponse = await FileHelper.Add(request.FormFile);
        var newDocument = new Document
        {
            Id = id,
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.Description,
            CreatedAt = document.Result.Data.CreatedAt
        };
        var result =await _repository.Update(id,newDocument);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }

        return new ErrorResponse<bool>(result);
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        var document = GetById(id);
        if (!document.Result.Success)
        {
            return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
        }
        var result = await _repository.Delete(id);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }

        return new ErrorResponse<bool>(result);
    }
}