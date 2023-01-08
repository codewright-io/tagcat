namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Interface for domain events
/// </summary>
public interface IDomainEvent
{
    /// <summary> identifier for the object (unique within the tenancy) </summary>
    string Id { get; }

    /// <summary>Tenant Id for the object</summary>
    string TenantId { get; }

    /// <summary>Time that the event was created</summary>
    DateTime Time { get; }

    /// <summary>The source service that generated the event</summary>
    string SourceId { get; }

    /// <summary>The ID of the user that generated the event</summary>
    string UserId { get; }

    /// <summary>The ID of the type of object that the domain event pertains to</summary>
    string TypeId { get; }

    /// <summary>Version for event ordering</summary>
    long Version { get; }
}
