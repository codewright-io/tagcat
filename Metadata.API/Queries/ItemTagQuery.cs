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
    private readonly IItemMetadataQuery _metadataQuery;
    private readonly IItemRelationshipQuery _relationshipQuery;
    private readonly IItemDetailQuery _detailQuery;

    /// <summary>
    /// Create an instance of a ItemTagQuery
    /// </summary>
    public ItemTagQuery(
        MetadataDbContext context, 
        IItemMetadataQuery metadataQuery, 
        IItemRelationshipQuery relationshipQuery,
        IItemDetailQuery detailQuery)
    {
        _context = context;
        _metadataQuery = metadataQuery;
        _relationshipQuery = relationshipQuery;
        _detailQuery = detailQuery;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemTagViewEntry>> FetchForIdAsync(string id, string tenantId, string culture)
    {
        // Get the item and its tags
        var matches = await _context.Relationships.AsNoTracking()
            .Where(item => item.Id == id)
            .Where(item => item.TenantId == tenantId)
            .Where(item => item.Type == RelationshipType.Tag)
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

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemResult>> GetItemsByTagAsync(
        string tenantId,
        string tag,
        string culture,
        string? metadataName,
        string? metadataValue,
        SearchOptions? options,
        int limit,
        int offset)
    {
        // First fetch the tag
        var tagIdMatches = await _metadataQuery.GetItemIdsByMetadataAsync(
            tenantId, MetadataNames.Name, tag, MetadataNames.Culture, culture, 1, 0);

        var tagId = tagIdMatches.FirstOrDefault();
        if (tagId == null)
            return Enumerable.Empty<ItemResult>();

        // Return items referencing this tag
        var matches = !string.IsNullOrEmpty(metadataName) && !string.IsNullOrEmpty(metadataValue) ? 
            await GetItemsByMetadataAndReferenceAsync(tenantId, tagId, metadataName, metadataValue, limit, offset) :
            await _relationshipQuery.GetReferencingAsync(tagId, tenantId, limit, offset);

        return matches;
    }

    private async Task<IEnumerable<ItemResult>> GetItemsByMetadataAndReferenceAsync(
        string tenantId,
        string targetId,
        string name,
        string value,
        int limit,
        int offset)
    {
        var resultIds = await _context.Relationships.AsNoTracking()
            .Where(r => r.TenantId == tenantId)
            .Where(r => r.TargetId == targetId)
            .Join(
                _context.Metadata.AsNoTracking()
                    .Where(m => m.TenantId == tenantId)
                    .Where(m => m.Value == value)
                    .Where(m => m.Name == name)
            , r => r.Id, m => m.Id, (r, m) => r.Id)
            .Take(limit)
            .Skip(offset)
            .ToListAsync();

        var results = await _detailQuery.GetByIdsAsync(resultIds, tenantId);

        return results;
    }
}
