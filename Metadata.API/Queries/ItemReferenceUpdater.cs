using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Events;
using CodeWright.Metadata.API.Queries.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

internal class ItemReferenceUpdater : 
    IEventHandler<ItemReferencesAddedEvent>, 
    IEventHandler<ItemReferencesRemovedEvent>
{
    private readonly MetadataDbContext _context;

    public ItemReferenceUpdater(MetadataDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ItemReferencesAddedEvent ev)
    {
        var addedEntities = ev.AddedReferences
            .Select(r => new ReferenceEntity { Id = ev.Id, TenantId = ev.TenantId, Version = ev.Version, Type = r.Type, TargetId = r.TargetId });

        await _context.References.AddRangeAsync(addedEntities);
        await _context.SaveChangesAsync();
    }

    public async Task HandleAsync(ItemReferencesRemovedEvent ev)
    {
        var removeReferences = ev.RemovedReferences.ToList();
        var itemEntities = await _context.References
            .Where(r => r.Id == ev.Id)
            .Where(r => r.TenantId == ev.TenantId)
            .ToListAsync();
        var removedEntities = itemEntities
            .Where(r => removeReferences.Contains(new Model.ReferenceEntry { Type = r.Type, TargetId = r.TargetId }));

        _context.References.RemoveRange(removedEntities);
        await _context.SaveChangesAsync();
    }
}
