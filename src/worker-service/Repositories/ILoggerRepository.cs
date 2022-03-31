using worker_service.Models;

namespace worker_service.Repositories;

public interface ILoggerRepository
{
    void Create(Log log);
}