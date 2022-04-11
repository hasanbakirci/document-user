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

    public async Task<List<T>> GetAll()
    {
        return await _collection.AsQueryable().ToListAsync();
    }

    public async Task<T> GetBy(Expression<Func<T, bool>> expression)
    {
        //var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(expression).FirstOrDefaultAsync();
    }

    public async Task<List<T>> FilterBy(Expression<Func<T, bool>> filter)
    {
        var documents = await _collection.FindAsync(filter);
        return documents.ToList();
    }

    public async Task<T> InsertOne(T document)
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

    public async Task<float> UpdateOne(Expression<Func<T, bool>> expression, params (Expression<Func<T, object>>, object)[] updatedProperties)
    {
        var update = Builders<T>.Update.Set(b => b.UpdatedAt, DateTime.Now);
        foreach (var (key,value) in updatedProperties)
        {
            update = update.Set(key, value);
        }

        var result = await _collection.UpdateOneAsync(expression, update);
        return result.ModifiedCount;
    }

    public async Task<float> ReplaceOne(Expression<Func<T, bool>> expression,T document)
    {
        document.UpdatedAt = DateTime.Now;
        var result = await _collection.ReplaceOneAsync(expression, document);
        return result.ModifiedCount;
    }

    public async Task<float> DeleteOne(Expression<Func<T, bool>> expression)
    {
        var result = await _collection.DeleteOneAsync(expression);
        return result.DeletedCount;
    }
}