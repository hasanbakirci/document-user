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
        var database = client.GetDatabase(settings.Database);
        _document = database.GetCollection<Document>(settings.Collection);
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

    public async Task<bool> Update(Guid id,Document document)
    {
        document.UpdatedAt = DateTime.UtcNow;
        var result = await _document.ReplaceOneAsync(d => d.Id == id, document);
        if (result.ModifiedCount > 0)
        {
            return true;
        }
        return false;
    }

    public async Task<bool> Delete(Guid id)
    {
        var result = await _document.DeleteOneAsync(d => d.Id == id);
        if (result.DeletedCount > 0)
        {
            return true;
        }
        return false;
    }
}