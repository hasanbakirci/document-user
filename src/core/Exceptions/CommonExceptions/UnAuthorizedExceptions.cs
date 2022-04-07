namespace core.Exceptions.CommonExceptions;


public class NullTokenException : ErrorDetails
{
    public NullTokenException() : base(401,4011, $"Token not found")
    {
    }
}

public class InvalidTokenException : ErrorDetails
{
    public InvalidTokenException() : base(401,4012, $"Invalid token")
    {
    }
}