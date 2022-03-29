using document_service.Models;
using document_service.Models.Dtos.Responses;

namespace document_service.Extensions;

public static class ConverterExtensions
{
    public static DocumentResponse ToDocumentResponse(this Document document)
    {
        return new DocumentResponse
        {
            Id = document.Id,
            Name = document.Name,
            Description = document.Description,
            Extension = document.Extension,
            Path = document.Path,
            CreatedAt = document.CreatedAt,
            UpdatedAt = document.UpdatedAt
        };
    }

    public static IEnumerable<DocumentResponse> ToDocumentsResponse(this IEnumerable<Document> documents)
    {
        List<DocumentResponse> documentResponses = new List<DocumentResponse>();
        documents.ToList().ForEach(
            d => documentResponses.Add(
                new DocumentResponse
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    Extension = d.Extension,
                    Path = d.Path,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                }));
        return documentResponses;
    }
}