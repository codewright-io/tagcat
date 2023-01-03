namespace CodeWright.Common.EventSourcing;

public class CurrentTimeProvider : ITimeProvider
{
    public DateTime GetCurrentTimeUtc() => DateTime.UtcNow;
}
