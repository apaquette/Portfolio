namespace Exceptions;

public class InvalidDateException : Exception
{
    public InvalidDateException(string msg)
    : base($"{msg}") { }
}