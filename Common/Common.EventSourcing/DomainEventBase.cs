namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Optional base class for domain events
/// </summary>
public abstract class DomainEventBase : IDomainEvent
{
    /// <inheritdoc />
    public string Id { get; init; } = string.Empty;

    /// <inheritdoc />
    public string TenantId { get; init; } = string.Empty;

    /// <inheritdoc />
    public DateTime Time { get; init; }

    /// <inheritdoc />
    public string SourceId { get; init; } = string.Empty;

    /// <inheritdoc />
    public long Version { get; init; }

    /// <summary>The ID of the user that generated the event</summary>
    public string UserId { get; init; } = string.Empty;

    /// <summary>The ID of the type of object that the domain event pertains to</summary>
    public abstract string TypeId { get; }

    /// <summary>The event class name, used to assist in deserializing</summary>
    public string EventClass => GetType().Name;
}
