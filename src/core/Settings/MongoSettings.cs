namespace core.Settings;

public class MongoSettings : IMongoSettings
{
    public string Server { get; set; }
    public string Database { get; set; }
    public string Collection { get; set; }
}