namespace GraderCommon.Exceptions;

public class GraderFileNotFoundException : GraderException
{
    public GraderFileNotFoundException(string message) : base(message)
    {
    }
}