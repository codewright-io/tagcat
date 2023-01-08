namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Bus to send domain events
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Send the domain event
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    Task SendAsync(IEnumerable<IDomainEvent> events);
}
