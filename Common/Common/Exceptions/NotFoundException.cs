namespace CodeWright.Common.Exceptions;

/// <summary>
/// Item as requested was not found (HTTP 404 error)
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException() : base("Not found") { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}

