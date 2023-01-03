using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Item detail queries
/// </summary>
public class ItemDetailQuery : IItemDetailQuery
{
    private readonly MetadataDbContext _context;

    /// <summary>
    /// Create an instance of a ItemDetailQuery
    /// </summary>
    public ItemDetailQuery(MetadataDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<ItemResult> GetByIdAsync(string id, string tenantId)
    {
        var results = await GetItemsByIdAsync(new List<string> { id }, tenantId);
        return results.First();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemResult>> GetItemsByIdAsync(IEnumerable<string> ids, string tenantId)
    {
        // Fetch the metadata for these IDs
        var itemMetadata = await _context.Metadata.AsNoTracking()
            .Where(m => ids.Contains(m.Id))
            .Where(m => m.TenantId == tenantId)
            .GroupBy(m => m.Id)
            .ToListAsync();

        // Fetch the relationships for these IDs
        var itemRelationships = await _context.Relationships.AsNoTracking()
            .Where(m => ids.Contains(m.Id))
            .Where(m => m.TenantId == tenantId)
            .GroupBy(m => m.Id)
            .ToListAsync();

        // Convert to results
        var results = ids
            .Select(id => new ItemResult 
            { 
                Id = id, 
                TenantId = tenantId,
                Metadata = itemMetadata
                    .Where(r => r.Key == id)
                    .SelectMany(m => m.Select(g => new MetadataEntry {  Name = g.Name, Value = g.Value })),
                Relationships = itemRelationships
                    .Where(r => r.Key == id)
                    .SelectMany(m => m.Select(g => new RelationshipEntry { TargetId = g.TargetId, Type = g.Type })),
            });

        return results;
    }
}
