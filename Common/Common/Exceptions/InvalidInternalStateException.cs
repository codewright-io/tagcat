namespace CodeWright.Common.Exceptions;

/// <summary>
/// Application reach a bad internal state due to a bug (HTTP 500 error)
/// </summary>
public class InvalidInternalStateException : Exception
{
    public InvalidInternalStateException() : base("Invalid internal state") { }

    public InvalidInternalStateException(string message) : base(message) { }

    public InvalidInternalStateException(string message, Exception innerException) : base(message, innerException) { }
}

