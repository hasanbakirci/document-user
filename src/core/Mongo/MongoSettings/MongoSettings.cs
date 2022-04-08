namespace core.Mongo.MongoSettings;

public class MongoSettings : IMongoSettings
{
    public string Server { get; set; }
    public string Database { get; set; }
}