using core.ServerResponse;
using document_service.Clients.MessageQueueClient;
using document_service.Extensions;
using document_service.Helpers;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Models.Dtos.Responses;
using document_service.Repositories;

namespace document_service.Services;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _repository;
    private readonly IMessageQueueClient _messageQueueClient;
    private readonly IUserService _userService;

    public DocumentService(IDocumentRepository repository, IMessageQueueClient messageQueueClient, IUserService userService)
    {
        _repository = repository;
        _messageQueueClient = messageQueueClient;
        _userService = userService;
    }

    public async Task<Response<IEnumerable<DocumentResponse>>> GetAll()
    {
        var documents = await _repository.GetAll();
        return new SuccessResponse<IEnumerable<DocumentResponse>>(documents.ToDocumentsResponse());
    }

    public async Task<Response<DocumentResponse>> GetById(Guid id)
    {
        var document = await _repository.GetById(id);
        if (document is not null)
        {
            return new SuccessResponse<DocumentResponse>(document.ToDocumentResponse());
        }
        
        return new ErrorResponse<DocumentResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
    }

    public async Task<Response<string>> Create(string token,CreateDocumentRequest request)
    {
        var fileResponse = await FileHelper.Add(request.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.LaterName
        };
        var result =await _repository.Create(document);
        var validateToken = _userService.ValidateToken(token);
        if (result != null && validateToken.Success)
        {
            
            _messageQueueClient.Publish(RabbitMQHelper.LoggerQueue,ConverterExtensions.CreateLog(document,validateToken.Data.Id));
            return new SuccessResponse<string>(result);
        }
        
        return new ErrorResponse<string>(ResponseStatus.BadRequest,result,ResultMessage.Error);


    }

    public async Task<Response<bool>> Update(string token,Guid id, UpdateDocumentRequest request)
    {
        var fileResponse = await FileHelper.Add(request.FormFile);
        var newDocument = new Document
        {
            Id = id,
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.Description,
        };
        var result =await _repository.Update(id,newDocument);
        var validateToken = _userService.ValidateToken(token);
        if (result && validateToken.Success)
        {
            
            _messageQueueClient.Publish(RabbitMQHelper.LoggerQueue,ConverterExtensions.CreateLog(newDocument, validateToken.Data.Id));
            return new SuccessResponse<bool>(result);
        }
        
        return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        var result = await _repository.Delete(id);
        if (result)
        {
            return new SuccessResponse<bool>(result);
        }
        
        return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
    }
}