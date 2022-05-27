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

public class UpdateDocumentCommand : IRequest<core.ServerResponse.Response<bool>>
{
    public Guid Id { get; set; }
    public UpdateDocumentRequest UpdateDocumentRequest { get; set; }
    public string Token { get; set; }
}

public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, core.ServerResponse.Response<bool>>
{
    private readonly IBus _bus;
    private readonly IDocumentRepository _repository;
    private readonly IUserService _userService;

    public UpdateDocumentCommandHandler(IBus bus, IDocumentRepository repository, IUserService userService)
    {
        _bus = bus;
        _repository = repository;
        _userService = userService;
    }

    public async Task<core.ServerResponse.Response<bool>> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
    {
        if (!MimeTypeIsValid(request.UpdateDocumentRequest.FormFile.ContentType))
            throw new MimeTypeException(request.UpdateDocumentRequest.FormFile.ContentType);
        
        var fileResponse = await FileHelper.Add(request.UpdateDocumentRequest.FormFile);
        var newDocument = new Document
        {
            Id = request.Id,
            Extension = fileResponse.FileExtension,
            Path = fileResponse.FilePath,
            Name = fileResponse.FileName,
            Description = request.UpdateDocumentRequest.Description,
            MimeType = request.UpdateDocumentRequest.FormFile.ContentType
        };
        
        var validateToken = _userService.ValidateToken(request.Token);
        var result = await _repository.UpdateDocument(request.Id, newDocument);

        if (result && validateToken.Success)
        {
            var log = ConverterExtensions.CreateLog(newDocument, validateToken.Data.Id);
            log.IsCreate = false;
            await _bus.Publish<IRequestDocumentEvent>(log);
            return new SuccessResponse<bool>(true);
        }
        
        throw new DocumentNotFound(request.Id);
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