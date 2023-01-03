namespace CodeWright.Common.Exceptions;

/// <summary>
/// A bad request was received (HTTP 400 error)
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(): base("Bad Request") { }

    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, Exception innerException) : base(message, innerException) { }
}

