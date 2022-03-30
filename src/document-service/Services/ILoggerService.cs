using document_service.Models.Dtos.Requests;

namespace document_service.Services;

public interface ILoggerService
{
    void SendLog(LogRequest request);
}