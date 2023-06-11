using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Queries.Interfaces;

/// <summary>
/// Metadata related queries
/// </summary>
public interface IItemMetadataQuery
{
    /// <summary>
    /// Fetch metadata by ID
    /// </summary>
    Task<IEnumerable<MetadataEntry>> FetchForIdAsync(string id, string tenantId);

    /// <summary>
    /// Search for items using metadata, returning IDs
    /// </summary>
    Task<IEnumerable<string>> GetItemIdsByMetadataAsync(
        string tenantId,
        string name,
        string value,
        string? secondaryName,
        string? secondaryValue,
        int limit,
        int offset);

    /// <summary>
    /// Search for items using metadata
    /// </summary>
    Task<IEnumerable<ItemResult>> GetItemsByMetadataAsync(
        string tenantId,
        string name,
        string value,
        string? secondaryName,
        string? secondaryValue,
        int limit,
        int offset);
}
