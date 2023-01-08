namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Class that provides the current item using the time provider interface
/// </summary>
public class CurrentTimeProvider : ITimeProvider
{
    /// <summary>
    /// Return the current UTC time
    /// </summary>
    /// <returns>The current UTC time</returns>
    public DateTime GetCurrentTimeUtc() => DateTime.UtcNow;
}
