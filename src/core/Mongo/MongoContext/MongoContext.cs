using MongoDB.Driver;

namespace core.Mongo.MongoContext;

public class MongoContext : IMongoContext
{
    private readonly IMongoDatabase _database;

    public MongoContext(MongoClient mongoClient, string databaseName)
    {
        _database = mongoClient.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collection)
    {
        return _database.GetCollection<T>(collection);
    }

    public IMongoDatabase GetDatabase()
    {
        return _database;
    }
}