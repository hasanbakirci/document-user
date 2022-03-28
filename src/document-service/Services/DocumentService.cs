using document_service.Helpers;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Repositories;

namespace document_service.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;

    public DocumentService(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Document>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Document> GetById(Guid id)
    {
        return await _repository.GetById(id);
    }

    public async Task<string> Create(CreateDocumentRequest request)
    {
        var fileResponse = await FileHelper.Add(request.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.LaterName
        };
        return await _repository.Create(document);
    }

    public async Task<bool> Update(Guid id, UpdateDocumentRequest request)
    {
        var fileResponse = await FileHelper.Add(request.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.Description
        };
        return await _repository.Update(document);
    }

    public async Task<bool> Delete(Guid id)
    {
        return await _repository.Delete(id);
    }
}