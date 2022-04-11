using core.Exceptions.CommonExceptions;
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
        var document = await _repository.GetBy(d => d.Id == id);
        if (document is not null)
        {
            return new SuccessResponse<DocumentResponse>(document.ToDocumentResponse());
        }
        
        //return new ErrorResponse<DocumentResponse>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
        throw new DocumentNotFound(id);
    }

    public async Task<Response<string>> Create(string token,CreateDocumentRequest request)
    {
        if (!MimeTypeIsValid(request.FormFile.ContentType))
            throw new MimeTypeException(request.FormFile.ContentType);
        
        var fileResponse = await FileHelper.Add(request.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.Description,
            MimeType = request.FormFile.ContentType
        };
        
        var validateToken = _userService.ValidateToken(token);
        var result =await _repository.InsertOne(document);
        
        //if (result != null && validateToken.Success)
        //{
            _messageQueueClient.Publish(RabbitMQHelper.LoggerQueue,ConverterExtensions.CreateLog(document,validateToken.Data.Id));
            return new SuccessResponse<string>(result.Id.ToString());
        //}
        
        //return new ErrorResponse<string>(ResponseStatus.BadRequest,result,ResultMessage.Error);


    }

    public async Task<Response<bool>> Update(string token,Guid id, UpdateDocumentRequest request)
    {
        if (!MimeTypeIsValid(request.FormFile.ContentType))
            throw new MimeTypeException(request.FormFile.ContentType);
        
        var fileResponse = await FileHelper.Add(request.FormFile);
        var newDocument = new Document
        {
            Id = id,
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.Description,
            MimeType = request.FormFile.ContentType
        };
        
        var validateToken = _userService.ValidateToken(token);
        var result =await _repository.UpdateOne(
            d => d.Id == id,
            (d => d.Description, newDocument.Description),
            (d => d.Extension, newDocument.Extension),
            (d => d.Path, newDocument.Path),
            (d => d.Name, newDocument.Name),
            (d => d.MimeType,newDocument.MimeType)
            );

        if (result > 0 && validateToken.Success)
        {
            _messageQueueClient.Publish(RabbitMQHelper.LoggerQueue,ConverterExtensions.CreateLog(newDocument, validateToken.Data.Id));
            return new SuccessResponse<bool>(true);
        }
        
        //return new ErrorResponse<bool>(ResponseStatus.NotFound, default, ResultMessage.NotFoundDocument);
        throw new DocumentNotFound(id);
    }

    public async Task<Response<bool>> Delete(Guid id)
    {
        var result = await _repository.DeleteOne(d => d.Id == id);
        if (result > 0)
        {
            return new SuccessResponse<bool>(true);
        }
        
        //return new ErrorResponse<bool>(ResponseStatus.NotFound, false, ResultMessage.NotFoundDocument);
        throw new DocumentNotFound(id);
    }
    
    private bool MimeTypeIsValid(string arg)
    {
        string[] mimeType = {"application/msword","application/vnd.openxmlformats-officedocument.wordprocessingml.document","text/plain"};
        /*
         * application/msword .doc
         * application/vnd.openxmlformats-officedocument.wordprocessingml.document .docx
         * text/plain .txt
         */
        var result = Array.Exists(mimeType, arg.Contains);
        return result;
    }
}