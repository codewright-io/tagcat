using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Events;
using CodeWright.Metadata.API.Queries.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

internal class ItemRelationshipUpdater : 
    IEventHandler<ItemRelationshipsAddedEvent>, 
    IEventHandler<ItemRelationshipsRemovedEvent>
{
    private readonly MetadataDbContext _context;

    public ItemRelationshipUpdater(MetadataDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(ItemRelationshipsAddedEvent ev)
    {
        var addedEntities = ev.AddedRelationships
            .Select(r => new RelationshipEntity { Id = ev.Id, TenantId = ev.TenantId, Version = ev.Version, Type = r.Type, TargetId = r.TargetId });

        await _context.Relationships.AddRangeAsync(addedEntities);
        await _context.SaveChangesAsync();
    }

    public async Task HandleAsync(ItemRelationshipsRemovedEvent ev)
    {
        var removeRelationships = ev.RemovedRelationships.ToList();
        var itemEntities = await _context.Relationships
            .Where(r => r.Id == ev.Id)
            .Where(r => r.TenantId == ev.TenantId)
            .ToListAsync();
        var removedEntities = itemEntities
            .Where(r => removeRelationships.Contains(new Model.RelationshipEntry { Type = r.Type, TargetId = r.TargetId }));

        _context.Relationships.RemoveRange(removedEntities);
        await _context.SaveChangesAsync();
    }
}
