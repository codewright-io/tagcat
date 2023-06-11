using CodeWright.Tagcat.API.Queries.Views;

namespace CodeWright.Tagcat.API.Queries.Interfaces;

/// <summary>
/// Tag related queries
/// </summary>
public interface IItemTagQuery
{
    /// <summary>
    /// Fetch tags for an ID
    /// </summary>
    Task<IEnumerable<ItemTagViewEntry>> FetchForIdAsync(string id, string tenantId, string culture);

    /// <summary>
    /// Search for items using a tag
    /// </summary>
    /// <param name="tenantId"></param>
    /// <param name="tag"></param>
    /// <param name="culture"></param>
    /// <param name="metadataName">An optional metadata name to match</param>
    /// <param name="metadataValue">An optional metadata value to match</param>
    /// <param name="options">Search options</param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    Task<IEnumerable<ItemResult>> GetItemsByTagAsync(
        string tenantId,
        string tag,
        string culture,
        string? metadataName,
        string? metadataValue,
        SearchOptions? options,
        int limit,
        int offset);
}
