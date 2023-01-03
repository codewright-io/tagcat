namespace CodeWright.Common.EventSourcing;

public class TimeVersionProvider : IVersionProvider
{
    private readonly ITimeProvider _timeProvider;

    public TimeVersionProvider(ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public long GetNewVersion()
    {
        var time = _timeProvider.GetCurrentTimeUtc();
        // TODO: Need to use server ID too? Or also a counter
        return time.Ticks;
    }
}
