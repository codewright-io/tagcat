using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Queries.Interfaces;

/// <summary>
/// Reference related queries
/// </summary>
public interface IItemReferenceQuery
{
    /// <summary>
    /// Fetch references for an ID
    /// </summary>
    Task<IEnumerable<ReferenceEntry>> FetchForIdAsync(string id, string tenantId);

    /// <summary>
    /// Get items that reference an item and return their IDs
    /// </summary>
    Task<IEnumerable<string>> GetReferencingIdsAsync(string targetId, string tenantId);

    /// <summary>
    /// Get items that reference an item
    /// </summary>
    Task<IEnumerable<ItemResult>> GetReferencingAsync(string targetId, string tenantId);
}
