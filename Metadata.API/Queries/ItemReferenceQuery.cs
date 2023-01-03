﻿using CodeWright.Metadata.API.Model;
using CodeWright.Metadata.API.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Reference related queries
/// </summary>
public class ItemReferenceQuery : IItemReferenceQuery
{
    private readonly MetadataDbContext _context;
    private readonly IItemDetailQuery _detailQuery;

    /// <summary>
    /// Create an instance of a ItemReferenceQuery
    /// </summary>
    public ItemReferenceQuery(MetadataDbContext context, IItemDetailQuery detailQuery)
    {
        _context = context;
        _detailQuery = detailQuery;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ReferenceEntry>> FetchForIdAsync(string id, string tenantId)
    {
        var matches = await _context.References.AsNoTracking()
            .Where(m => m.Id == id && m.TenantId == tenantId)
            .ToListAsync();

        return matches.Select(m => new ReferenceEntry { Type = m.Type, TargetId = m.TargetId });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetReferencingIdsAsync(string targetId, string tenantId)
    {
        var matchIds = await _context.References.AsNoTracking()
            .Where(m => m.TargetId == targetId && m.TenantId == tenantId)
            .Select(m => m.Id)
            .Distinct()
            .ToListAsync();

        return matchIds;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemResult>> GetReferencingAsync(string targetId, string tenantId)
    {
        var itemIds = await GetReferencingIdsAsync(targetId, tenantId);

        // Convert to results
        var results = await _detailQuery.GetItemsByIdAsync(itemIds, tenantId);
        return results;
    }
}
