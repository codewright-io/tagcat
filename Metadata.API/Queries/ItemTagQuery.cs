using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries.Interfaces;
using CodeWright.Metadata.API.Queries.Views;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Tag related queries
/// </summary>
public class ItemTagQuery : IItemTagQuery
{
    private readonly MetadataDbContext _context;

    /// <summary>
    /// Create an instance of a ItemTagQuery
    /// </summary>
    public ItemTagQuery(MetadataDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemTagViewEntry>> FetchForIdAsync(string id, string tenantId, string culture)
    {
        // Get the item and its tags
        var matches = await _context.References.AsNoTracking()
            .Where(item => item.Id == id)
            .Where(item => item.TenantId == tenantId)
            .Where(item => item.Type == ReferenceType.Tag)
            .Join(_context.Metadata.AsNoTracking(), item => item.TargetId, tagMetadata => tagMetadata.Id, 
                (item, tagMetadata) => new { item, tagMetadata })
            .ToListAsync();

        // Just filter by display name for now
        // TODO: Need to support tag culture
        matches = matches
            .Where(result => result.tagMetadata?.Name != null)
            .Where(result => result.tagMetadata?.Name == MetadataNames.Name)
            .ToList();

        return matches.Select(m => new ItemTagViewEntry 
        { 
            Id = m.tagMetadata.Id, 
            DisplayName = m.tagMetadata.Value
        });
    }
}
