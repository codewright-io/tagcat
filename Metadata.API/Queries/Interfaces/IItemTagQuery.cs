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
}
