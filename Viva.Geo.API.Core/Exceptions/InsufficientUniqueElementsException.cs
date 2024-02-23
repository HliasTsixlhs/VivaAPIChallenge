namespace Viva.Geo.API.Core.Exceptions;

public class InsufficientUniqueElementsException : Exception
{
    public InsufficientUniqueElementsException(string message) : base(message) {}
}