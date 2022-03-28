using document_service.Models;
using document_service.Models.Dtos.Requests;

namespace document_service.Services;

public interface IDocumentService
{
    Task<IEnumerable<Document>> GetAll();
    Task<Document> GetById(Guid id);
    Task<string> Create(CreateDocumentRequest request);
    Task<bool> Update(Guid id,UpdateDocumentRequest request);
    Task<bool> Delete(Guid id);
}