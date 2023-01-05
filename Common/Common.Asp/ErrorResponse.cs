using System.Diagnostics;

namespace CodeWright.Common.Asp;

public class ErrorResponse
{
    public ErrorResponse() { }

    public ErrorResponse(Exception ex, bool includeStackTrace)
    {
        Type = ex.GetType().Name;
        Message = ex.Message;
        StackTrace = ex.ToString();
    }

    public string Type { get; init; } = string.Empty;

    public string Message { get; init; } = string.Empty;

    public string StackTrace { get; init; } = string.Empty;
}
