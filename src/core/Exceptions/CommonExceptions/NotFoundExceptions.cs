namespace core.Exceptions.CommonExceptions;

public class DocumentNotFound : NotFound
{
    public DocumentNotFound(Guid id) : base(4040, $"- {id} was not found.")
    {
    }
}