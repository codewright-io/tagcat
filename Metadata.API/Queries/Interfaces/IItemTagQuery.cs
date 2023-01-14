using CodeWright.Metadata.API.Queries.Views;

namespace CodeWright.Metadata.API.Queries.Interfaces;

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
    /// <param name="type">An optional type to filter on</param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    Task<IEnumerable<ItemResult>> GetItemsByTagAsync(
        string tenantId,
        string tag,
        string culture,
        string? type,
        int limit,
        int offset);
}
