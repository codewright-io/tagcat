namespace CodeWright.Common.EventSourcing;

public interface IDomainObject
{
    /// <summary>
    /// Unique identifier for the object (unique within the tenancy)
    /// </summary>
    string Id { get; init; }

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    string TenantId { get; init; }

    /// <summary>
    /// Version for event ordering
    /// </summary>
    long Version { get; }

    /// <summary>A unique ID for the type</summary>
    string TypeId { get; }

    /// <summary>
    /// Start queueing events.
    /// </summary>
    void StartQueuing();

    /// <summary>
    /// Stop queueing events and fetch the current list.
    /// </summary>
    IEnumerable<IDomainEvent> StopQueuing();
}
