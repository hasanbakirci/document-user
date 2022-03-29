using Core.Repositories.Settings;
using document_service.Models;
using MongoDB.Driver;

namespace document_service.Repositories;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<Document> _document;
    public DocumentRepository(IMongoSettings settings)
    {
        var client = new MongoClient(settings.Server);
        var databse = client.GetDatabase(settings.Database);
        _document = databse.GetCollection<Document>(settings.Collection);
    }
    public async Task<IEnumerable<Document>> GetAll()
    {
        return await _document.Find(d => true).ToListAsync();
    }

    public async Task<Document> GetById(Guid id)
    {
        var document = await _document.FindAsync(d => d.Id == id);
        return document.FirstOrDefault();
    }

    public async Task<string> Create(Document document)
    {
        document.CreatedAt = DateTime.UtcNow;
        await _document.InsertOneAsync(document);
        return document.Id.ToString();
    }

    public async Task<bool> Update(Document document)
    {
        document.UpdatedAt = DateTime.UtcNow;
        var result = await _document.FindOneAndReplaceAsync(d => d.Id == document.Id, document);
        return result is not null ? true : false;
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _document.DeleteOneAsync(d => d.Id == id);
        return result.DeletedCount == 1 ? true : false;
    }
}