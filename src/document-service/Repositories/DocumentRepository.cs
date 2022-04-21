using System.Linq.Expressions;
using core.Mongo.MongoContext;
using core.Mongo.MongoRepository;
using core.Mongo.MongoSettings;
using document_service.Models;
using MongoDB.Driver;

namespace document_service.Repositories;

public class DocumentRepository : MongoRepository<Document>,IDocumentRepository
{
    public DocumentRepository(IMongoContext mongoContext, string collection = "documents") : base(mongoContext,collection) { }
    
    public async Task<Document> GetById(Guid id)
    {
        return await GetBy(d => d.Id == id);
    }

    public async Task<bool> DeleteById(Guid id)
    {
        var result = await DeleteOne(d => d.Id == id);
        if (result > 0)
            return true;
        return false;
    }

    public async Task<bool> UpdateDocument(Guid id, Document document)
    {
        var filter = new[]
        {
            ((Expression<Func<Document, object>>, object)) 
            (d => d.Extension, document.Extension),
            (d => d.Description, document.Description),
            (d => d.Path, document.Path),
            (d => d.Name, document.Name),
            (d => d.MimeType, document.MimeType)
        };
        var result = await UpdateOne(d => d.Id == id, filter);
        if (result > 0)
            return true;
        return false;
    }


    // private readonly IMongoCollection<Document> _document;
    // public DocumentRepository(IMongoSettings settings)
    // {
    //     var client = new MongoClient(settings.Server);
    //     var database = client.GetDatabase(settings.Database);
    //     _document = database.GetCollection<Document>("Documents");
    // }
    // public async Task<IEnumerable<Document>> GetAll()
    // {
    //     return await _document.Find(d => true).ToListAsync();
    // }
    //
    // public async Task<Document> GetById(Guid id)
    // {
    //     var document = await _document.FindAsync(d => d.Id == id);
    //     return document.FirstOrDefault();
    // }
    //
    // public async Task<string> Create(Document document)
    // {
    //     document.CreatedAt = DateTime.UtcNow;
    //     document.UpdatedAt = DateTime.UtcNow;
    //     await _document.InsertOneAsync(document);
    //     return document.Id.ToString();
    // }
    //
    // public async Task<bool> Update(Guid id,Document document)
    // {
    //     var filter = Builders<Document>.Filter.Eq(d => d.Id, id);
    //     var update = Builders<Document>.Update
    //         .Set(d => d.Name, document.Name)
    //         .Set(d => d.Path, document.Path)
    //         .Set(d => d.Extension,document.Extension)
    //         .Set(d => d.Description,document.Description)
    //         .Set(d => d.UpdatedAt, DateTime.UtcNow);
    //     var result = await _document.ReplaceOneAsync(d => d.Id == id, document);
    //     
    //     if (result.ModifiedCount > 0)
    //     {
    //         return true;
    //     }
    //     return false;
    // }
    //
    // public async Task<bool> Delete(Guid id)
    // {
    //     var result = await _document.DeleteOneAsync(d => d.Id == id);
    //     if (result.DeletedCount > 0)
    //     {
    //         return true;
    //     }
    //     return false;
    // }
}