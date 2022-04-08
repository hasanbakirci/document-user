using System.Linq.Expressions;
using core.Mongo.MongoContext;
using MongoDB.Driver;

namespace core.Mongo.MongoRepository;

public class MongoRepository<T> : IMongoRepository<T> where T : BaseDocument
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoContext mongoContext, string collection = "")
    {
        if (string.IsNullOrEmpty(collection))
        {
            collection = typeof(T).Name;
        }

        collection = collection.First().ToString().ToLower() + collection.Substring(1);
        _collection = mongoContext.GetCollection<T>(collection);
    }

    public List<T> FindAll()
    {
        return _collection.AsQueryable().ToList();
    }

    public async Task<List<T>> FindAllAsync()
    {
        return await _collection.AsQueryable().ToListAsync();
    }

    public T FindById(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return _collection.Find(filter).FirstOrDefault();
    }

    public async Task<T> FindByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public List<T> FilterBy(Expression<Func<T, bool>> filter)
    {
        return _collection.Find(filter).ToList();
    }

    public async Task<List<T>> FilterByAsync(Expression<Func<T, bool>> filter)
    {
        var documents = await _collection.FindAsync(filter);
        return documents.ToList();
    }

    public T InsertOne(T document)
    {
        if (document.Id == null)
        {
            document.Id = Guid.NewGuid();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        _collection.InsertOne(document);
        return document;
    }

    public async Task<T> InsertOneAsync(T document)
    {
        if (document.Id == null)
        {
            document.Id = Guid.NewGuid();
        }
        document.CreatedAt = DateTime.Now;
        document.UpdatedAt = DateTime.Now;
        await _collection.InsertOneAsync(document);
        return document;
    }

    public bool UpdateOne(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties)
    {
        var update = Builders<T>.Update.Set(b => b.UpdatedAt, DateTime.Now);
        foreach (var (key,value) in updatedProperties)
        {
            update = update.Set(key, value);
        }

        var result = _collection.UpdateOne(expression, update);
        return result.ModifiedCount > 0 ? true : false;
    }

    public async Task<bool> UpdateOneAsync(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties)
    {
        var update = Builders<T>.Update.Set(b => b.UpdatedAt, DateTime.Now);
        foreach (var (key,value) in updatedProperties)
        {
            update = update.Set(key, value);
        }

        var result = await _collection.UpdateOneAsync(expression, update);
        return result.ModifiedCount > 0 ? true : false;
    }

    public bool ReplaceOne(Guid id,T document)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = _collection.ReplaceOne(filter, document);
        return result.ModifiedCount > 0 ? true : false;
    }

    public async Task<bool> ReplaceOneAsync(Guid id,T document)
    {
        document.UpdatedAt = DateTime.Now;
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.ReplaceOneAsync(filter, document);
        return result.ModifiedCount > 0 ? true : false;
    }

    public bool DeleteOne(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = _collection.DeleteOne(filter);
        return result.DeletedCount > 0 ? true : false;
    }

    public async Task<bool> DeleteOneAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        var result = await _collection.DeleteOneAsync(filter);
        return result.DeletedCount > 0 ? true : false;
    }
}