using core.Mongo.MongoContext;
using core.Mongo.MongoRepository;
using worker_service.Models;

namespace worker_service.Repositories;

public class LoggerRepository :MongoRepository<Log>,ILoggerRepository
{
    public LoggerRepository(IMongoContext mongoContext, string collection = "logs"): base(mongoContext,collection) { }
    // private readonly IMongoCollection<Log> _log;
    //
    // public LoggerRepository(IMongoSettings settings)
    // {
    //     var client = new MongoClient(settings.Server);
    //     var database = client.GetDatabase(settings.Database);
    //     _log = database.GetCollection<Log>("Logs");
    // }
    // public void Create(Log log)
    // {
    //     _log.InsertOneAsync(log);
    // }
}