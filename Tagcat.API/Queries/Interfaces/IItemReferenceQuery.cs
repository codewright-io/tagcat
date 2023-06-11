using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Queries.Interfaces;

/// <summary>
/// Relationship related queries
/// </summary>
public interface IItemRelationshipQuery
{
    /// <summary>
    /// Fetch relationship for an ID
    /// </summary>
    Task<IEnumerable<RelationshipEntry>> FetchForIdAsync(string id, string tenantId);

    /// <summary>
    /// Get items that relationship an item and return their IDs
    /// </summary>
    Task<IEnumerable<string>> GetReferencingIdsAsync(string targetId, string tenantId, int limit, int offset);

    /// <summary>
    /// Get items that relationship an item
    /// </summary>
    Task<IEnumerable<ItemResult>> GetReferencingAsync(string targetId, string tenantId, int limit, int offset);
}
