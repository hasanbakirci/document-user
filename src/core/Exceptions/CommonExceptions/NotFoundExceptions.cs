namespace core.Exceptions.CommonExceptions;

public class DocumentNotFound : ErrorDetails
{
    public DocumentNotFound(Guid id) : base(404,4040, $" {id} was not found.")
    {
    }
}