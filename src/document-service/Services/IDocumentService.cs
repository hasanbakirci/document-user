using core.ServerResponse;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;

namespace document_service.Services;

public interface IDocumentService
{
    Task<Response<IEnumerable<DocumentResponse>>> GetAll();
    Task<Response<DocumentResponse>> GetById(Guid id);
    Task<Response<string>> Create(CreateDocumentRequest request);
    Task<Response<bool>> Update(Guid id,UpdateDocumentRequest request);
    Task<Response<bool>> Delete(Guid id);
}