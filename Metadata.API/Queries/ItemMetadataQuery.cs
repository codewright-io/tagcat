using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Metadata related queries
/// </summary>
public class ItemMetadataQuery : IItemMetadataQuery
{
    private readonly MetadataDbContext _context;
    private readonly IItemDetailQuery _detailQuery;

    /// <summary>
    /// Create an instance of a ItemMetadataQuery
    /// </summary>
    public ItemMetadataQuery(MetadataDbContext context, IItemDetailQuery detailQuery)
    {
        _context = context;
        _detailQuery = detailQuery;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<MetadataEntry>> FetchForIdAsync(string id, string tenantId)
    {
        var matches = await _context.Metadata.AsNoTracking()
            .Where(m => m.Id == id && m.TenantId == tenantId)
            .ToListAsync();

        return matches.Select(m => new MetadataEntry { Name = m.Name, Value = m.Value });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetItemIdsByMetadataAsync(
        string tenantId,
        string name,
        string value,
        string? secondaryName,
        string? secondaryValue,
        int limit,
        int offset)
    {
        var valueQuery = _context.Metadata.AsNoTracking()
            .Where(m => m.TenantId == tenantId)
            .Where(m => m.Value == value)
            .Where(m => m.Name == name)
            .Select(m => m.Id)
            .Distinct();

        var idQuery = valueQuery.Distinct();
        if (!string.IsNullOrEmpty(secondaryName) && !string.IsNullOrEmpty(secondaryValue))
        {
            idQuery = idQuery.Join(
                _context.Metadata.AsNoTracking()
                    .Where(m => m.TenantId == tenantId)
                    .Where(m => m.Value == secondaryValue)
                    .Where(m => m.Name == secondaryName),
                m => m, m => m.Id, (m1, m2) => m1);
        }

        // Search on the primary name and value first, join with all metadata and search on the secondary query if applicable
        var itemIds = await idQuery
            .Skip(offset)
            .Take(limit)
            .ToListAsync();

        return itemIds;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemResult>> GetItemsByMetadataAsync(
        string tenantId, 
        string name,
        string value,
        string? secondaryName,
        string? secondaryValue,
        int limit,
        int offset)
    {
        var itemIds = await GetItemIdsByMetadataAsync(tenantId, name, value, secondaryName, secondaryValue, limit, offset);

        // Convert to results
        var results = await _detailQuery.GetItemsByIdAsync(itemIds, tenantId);

        return results;
    }
}
