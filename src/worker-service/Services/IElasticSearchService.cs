using worker_service.Models;

namespace worker_service.Services;

public interface IElasticSearchService
{
    string InsertLog(Log log);
}