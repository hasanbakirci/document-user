using core.Mongo.MongoRepository;
using document_service.Models;

namespace document_service.Repositories;

public interface IDocumentRepository : IMongoRepository<Document>
{
    // Task<IEnumerable<Document>> GetAll();
    //Task<Document> GetById(Guid id);
    // Task<string> Create(Document document);
    // Task<bool> Update(Guid id,Document document);
    // Task<bool> Delete(Guid id);
    
    
    Task<Document> GetById(Guid id);
    Task<bool> DeleteById(Guid id);
    Task<bool> UpdateDocument(Guid id, Document document);
}