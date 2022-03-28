using document_service.Models;

namespace document_service.Repositories;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetAll();
    Task<Document> GetById(Guid id);
    Task<string> Create(Document document);
    Task<bool> Update(Document document);
    Task<bool> Delete(Guid id);
}