using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;

namespace document_service.Services;

public interface IDocumentService
{
    Task<IEnumerable<DocumentResponse>> GetAll();
    Task<DocumentResponse> GetById(Guid id);
    Task<string> Create(CreateDocumentRequest request);
    Task<bool> Update(Guid id,UpdateDocumentRequest request);
    Task<bool> Delete(Guid id);
}