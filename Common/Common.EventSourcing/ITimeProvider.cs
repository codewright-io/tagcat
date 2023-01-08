namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Interface to provide the current time
/// </summary>
/// <remarks>
/// This is interfaced to allow unit tests to provide the time.
/// </remarks>
public interface ITimeProvider
{
    /// <inheritdoc/>
    DateTime GetCurrentTimeUtc();
}
