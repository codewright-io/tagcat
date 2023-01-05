using CodeWright.Common.EventSourcing.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CodeWright.Common.EventSourcing.EntityFramework;

/// <summary>
/// An entity framework based event store.
/// </summary>
public class EFEventStore : IEventStore
{
    private readonly EventSourceDbContext _context;
    private readonly JsonConverter _converter;

    public EFEventStore(EventSourceDbContext context, JsonConverter converter)
    {
        _context = context;
        _converter = converter;
    }

    public async Task<IEnumerable<IDomainEvent>> GetByIdAsync(string id, string tenantId, long fromVersion, int limit)
    {
        if (limit <= 0)
            throw new ArgumentException("Must be greater than zero", nameof(limit));

        // Construct an ID-based query 
        var eventQuery = _context.Events.AsNoTracking()
            .Where(ev => ev.Id == id && ev.TenantId == tenantId);

        // If a version was provided, then filter by it
        if (fromVersion > 0)
            eventQuery = eventQuery.Where(ev => ev.Version > fromVersion);

        // No order and limit the query
        var domainEventEntities = await eventQuery
            .OrderBy(ev => ev.Version)
            .Select(ev => ev.Content)
            .Take(limit)
            .ToListAsync();

        var domainEvents = domainEventEntities
            .Select(ev => JsonConvert.DeserializeObject<IDomainEvent>(ev, _converter))
            .Where(ev => ev != null)
            .OfType<IDomainEvent>();

        return domainEvents;
    }

    public async Task SaveAsync(IEnumerable<IDomainEvent> events)
    {
        var entities = events
            .Select(ev => new EventLogEntity
            {
                Id = ev.Id,
                TenantId = ev.TenantId,
                Version = ev.Version,
                Content = JsonConvert.SerializeObject(ev),
                CreateTime = ev.Time,
                SourceId = ev.SourceId,
                TypeId = ev.TypeId,
                UserId = ev.UserId,
            });

        await _context.Events.AddRangeAsync(entities);

        await _context.SaveChangesAsync();
    }
}
