﻿using CodeWright.Tagcat.API.Model;
using CodeWright.Tagcat.API.Queries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodeWright.Tagcat.API.Queries;

/// <summary>
/// Relationship related queries
/// </summary>
public class ItemRelationshipQuery : IItemRelationshipQuery
{
    private readonly MetadataDbContext _context;
    private readonly IItemDetailQuery _detailQuery;

    /// <summary>
    /// Create an instance of a ItemRelationshipQuery
    /// </summary>
    public ItemRelationshipQuery(MetadataDbContext context, IItemDetailQuery detailQuery)
    {
        _context = context;
        _detailQuery = detailQuery;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RelationshipEntry>> FetchForIdAsync(string id, string tenantId)
    {
        var matches = await _context.Relationships.AsNoTracking()
            .Where(m => m.Id == id && m.TenantId == tenantId)
            .ToListAsync();

        return matches.Select(m => new RelationshipEntry { Type = m.Type, TargetId = m.TargetId });
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<string>> GetReferencingIdsAsync(
        string targetId, string tenantId, int limit, int offset)
    {
        var matchIds = await _context.Relationships.AsNoTracking()
            .Where(m => m.TargetId == targetId && m.TenantId == tenantId)
            .Select(m => m.Id)
            .Distinct()
            .ToListAsync();

        return matchIds;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ItemResult>> GetReferencingAsync(string targetId, string tenantId, int limit, int offset)
    {
        var itemIds = await GetReferencingIdsAsync(targetId, tenantId, limit, offset);

        // Convert to results
        var results = await _detailQuery.GetByIdsAsync(itemIds, tenantId);
        return results;
    }
}
