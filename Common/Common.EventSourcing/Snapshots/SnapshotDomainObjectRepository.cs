namespace CodeWright.Common.EventSourcing.Snapshots;

/// <summary>
/// Similar to the BasicDomainObjectRepository, but adds snapshots for performance.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TFactory"></typeparam>
public class SnapshotDomainObjectRepository<T, TFactory> : IDomainRepository<T>
        where T : IDomainObject
        where TFactory : IDomainObjectFactory<T>, new()
{
    private readonly ISnapshotRepository<T> _snapshotStore;
    private readonly IEventStore _eventStore;
    private readonly IEventBus _eventBus;
    private readonly int _snapshotInterval;

    public SnapshotDomainObjectRepository(
        ISnapshotRepository<T> snapshotStore, 
        IEventStore eventStore,
        IEventBus eventBus,
        int snapshotInterval = 12)
    {
        if (snapshotInterval <= 0)
            throw new ArgumentOutOfRangeException(nameof(snapshotInterval), "Must be greater than 0");

        _snapshotStore = snapshotStore;
        _eventStore = eventStore;
        _eventBus = eventBus;
        _snapshotInterval = snapshotInterval;
    }

    public async Task<T?> GetByIdAsync(string id, string tenantId)
    {
        // Get the snapshot
        var snapshot = await _snapshotStore.GetAsync(id, tenantId);
        long snapshotVersion = (snapshot != null) ? snapshot.Version : -1;

        var events = await _eventStore.GetByIdAsync(id, tenantId, snapshotVersion, int.MaxValue);

        var builder = new TFactory();
        var item = (snapshot != null) ? builder.UpdateFromEvents(snapshot.Model, events) : builder.CreateFromEvents(events);

        // Take a snapshot for the next query if there were more than {snapshotInterval} events
        if (item != null && events.Count() > _snapshotInterval)
        {
            await _snapshotStore.SaveAsync(item, events.Last().Version);
        }

        item?.StartQueuing();

        return item;
    }

    /// <summary>
    /// Save the domain entity
    /// </summary>
    /// <param name="item">The domain object to save</param>
    /// <param name="userId">The user the made the changes</param>
    public async Task SaveAsync(T item, string userId)
    {
        var domainEvents = item.StopQueuing();

        // Save to event store
        await _eventStore.SaveAsync(domainEvents);

        // Send to event bus
        await _eventBus.SendAsync(domainEvents);
    }
}
