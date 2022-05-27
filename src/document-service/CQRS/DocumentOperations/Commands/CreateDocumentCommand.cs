using core.Exceptions.CommonExceptions;
using core.Masstransit.Events;
using core.ServerResponse;
using document_service.Extensions;
using document_service.Helpers;
using document_service.Models;
using document_service.Models.Dtos.Requests;
using document_service.Repositories;
using document_service.Services;
using MassTransit;
using MediatR;

namespace document_service.CQRS.DocumentOperations.Commands;

public class CreateDocumentCommand : IRequest<core.ServerResponse.Response<string>>
{
    public CreateDocumentRequest CreateDocumentRequest { get; set; }
    public string Token { get; set; }
}

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, core.ServerResponse.Response<string>>
{
    private readonly IDocumentRepository _repository;
    private readonly IUserService _userService;
    private readonly IBus _bus;

    public CreateDocumentCommandHandler(IDocumentRepository repository, IUserService userService, IBus bus)
    {
        _repository = repository;
        _userService = userService;
        _bus = bus;
    }

    public async Task<core.ServerResponse.Response<string>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!MimeTypeIsValid(request.CreateDocumentRequest.FormFile.ContentType))
            throw new MimeTypeException(request.CreateDocumentRequest.FormFile.ContentType);
        
        var fileResponse = await FileHelper.Add(request.CreateDocumentRequest.FormFile);
        var document = new Document
        {
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.CreateDocumentRequest.Description,
            MimeType = request.CreateDocumentRequest.FormFile.ContentType
        };
        
        var validateToken = _userService.ValidateToken(request.Token);
        var result =await _repository.InsertOne(document);
        
        //if (result != null && validateToken.Success)
        //{
        var log = ConverterExtensions.CreateLog(document, validateToken.Data.Id);
        //_messageQueueClient.Publish(RabbitMQHelper.LoggerQueue,ConverterExtensions.CreateLog(document,validateToken.Data.Id));
        log.IsCreate = true;
        Console.WriteLine(log.IsCreate);
        await _bus.Publish<IRequestDocumentEvent>(log);
        return new SuccessResponse<string>(result.Id.ToString());
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