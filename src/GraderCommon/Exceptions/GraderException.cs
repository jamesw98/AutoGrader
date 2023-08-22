namespace GraderCommon.Exceptions;

public class GraderException : Exception
{
    public GraderException() : base()
    {
    }

    public GraderException(string message) : base(message)
    {
    }

    public GraderException(string message, Exception inner) : base(message, inner)
    {
    }
}