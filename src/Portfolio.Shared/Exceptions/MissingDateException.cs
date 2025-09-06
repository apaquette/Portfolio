namespace Exceptions;

public class MissingDateException : Exception
{
    public MissingDateException(string msg)
    : base($"{msg}") { }
}