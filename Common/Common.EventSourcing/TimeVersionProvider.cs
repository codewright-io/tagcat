namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Version provider that uses the current ticks to generate a version
/// </summary>
public class TimeVersionProvider : IVersionProvider
{
    private readonly ITimeProvider _timeProvider;

    /// <summary>
    /// Create an instance of a TimeVersionProvider
    /// </summary>
    /// <param name="timeProvider"></param>
    public TimeVersionProvider(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    /// <inheritdoc/>
    public long GetNewVersion()
    {
        var time = _timeProvider.GetCurrentTimeUtc();
        // TODO: Need to use server ID too? Or also a counter
        return time.Ticks;
    }
}
