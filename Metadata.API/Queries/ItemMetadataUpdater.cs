using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Events;
using CodeWright.Metadata.API.Queries.Entities;

namespace CodeWright.Metadata.API.Queries;

internal class ItemMetadataUpdater : 
    IEventHandler<ItemMetadataAddedEvent>, 
    IEventHandler<ItemMetadataRemovedEvent>
{
    private readonly MetadataDbContext _context;

    public ItemMetadataUpdater(MetadataDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ItemMetadataAddedEvent ev)
    {
        var addedEntities = ev.AddedMetadata
            .Select(m => new MetadataEntity { Id = ev.Id, TenantId = ev.TenantId, Version = ev.Version, Name = m.Name, Value = m.Value });

        await _context.Metadata.AddRangeAsync(addedEntities);
        await _context.SaveChangesAsync();
    }

    public async Task HandleAsync(ItemMetadataRemovedEvent ev)
    {
        var removeNames = ev.RemovedMetadata.Select(m => m.Name).ToList();
        var removedEntities = _context.Metadata
            .Where(m => m.Id == ev.Id)
            .Where(m => m.TenantId == ev.TenantId)
            .Where(m => removeNames.Contains(m.Name));

        _context.Metadata.RemoveRange(removedEntities);
        await _context.SaveChangesAsync();
    }
}
