using worker_service.Extensions;
using worker_service.Models.Requests;
using worker_service.Repositories;

namespace worker_service.Services;

public class LoggerService: ILoggerService
{
    private readonly ILoggerRepository _loggerRepository;

    public LoggerService(ILoggerRepository loggerRepository)
    {
        _loggerRepository = loggerRepository;
    }

    public Guid Create(CreateLogRequest request)
    {
        var log =_loggerRepository.InsertOne(request.ToLog());
        return log.Result.Id;
    }
}