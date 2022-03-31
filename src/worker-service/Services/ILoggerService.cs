using worker_service.Models.Requests;

namespace worker_service.Services;

public interface ILoggerService
{
    void Create(CreateLogRequest request);
}