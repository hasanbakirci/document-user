using core.Mongo.MongoRepository;
using worker_service.Models;

namespace worker_service.Repositories;

public interface ILoggerRepository : IMongoRepository<Log>
{
    //void Create(Log log);
}