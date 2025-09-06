namespace Exceptions;

public class MissingGradDateException : Exception
{
    public MissingGradDateException()
    : base($"Degree must have a graduation date") { }
}