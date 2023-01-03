namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Common interface for classes that receive and process events.
/// </summary>
/// <typeparam name="TEvent">The type of event to receive</typeparam>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
public interface IEventHandler<TEvent>
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    where TEvent : IDomainEvent, new()
{
    Task HandleAsync(TEvent ev);
}
