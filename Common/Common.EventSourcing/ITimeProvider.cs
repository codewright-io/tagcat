namespace CodeWright.Common.EventSourcing;

public interface ITimeProvider
{
    DateTime GetCurrentTimeUtc();
}
