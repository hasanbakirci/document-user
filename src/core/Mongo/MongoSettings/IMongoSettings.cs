namespace core.Mongo.MongoSettings;

public interface IMongoSettings
{
    string Server { get; set; }
    string Database { get; set; }
}