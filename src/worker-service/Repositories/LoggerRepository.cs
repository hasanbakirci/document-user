using Core.Repositories.Settings;
using MongoDB.Driver;
using worker_service.Models;

namespace worker_service.Repositories;

public class LoggerRepository :ILoggerRepository
{
    private readonly IMongoCollection<Log> _log;

    public LoggerRepository(IMongoSettings settings)
    {
        var client = new MongoClient(settings.Server);
        var database = client.GetDatabase(settings.Database);
        _log = database.GetCollection<Log>(settings.Collection);
    }
    public void Create(Log log)
    {
        _log.InsertOneAsync(log);
    }
}