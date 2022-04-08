using MongoDB.Driver;

namespace core.Mongo.MongoContext;

public interface IMongoContext
{
    IMongoCollection<T> GetCollection<T>(string collection); // where T : Document;
    IMongoDatabase GetDatabase();
}