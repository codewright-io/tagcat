namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Interface for an event sourcing store
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// Fetch a list of events for the specified ID, ordered by version.
    /// </summary>
    /// <param name="id">The ID of the object.</param>
    /// <param name="tenantId">The ID of the tenant.</param>
    /// <param name="fromVersion">The version to start from.</param>
    /// <param name="limit">The maximum number of events to fetch</param>
    /// <returns>The list of events</returns>
    Task<IEnumerable<IDomainEvent>> GetByIdAsync(string id, string tenantId, long fromVersion, int limit);

    /// <summary>
    /// Save a list of events
    /// </summary>
    /// <param name="events"></param>
    /// <returns></returns>
    Task SaveAsync(IEnumerable<IDomainEvent> events);
}
