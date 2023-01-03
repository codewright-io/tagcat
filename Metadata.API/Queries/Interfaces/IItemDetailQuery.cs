namespace CodeWright.Metadata.API.Queries.Interfaces;

/// <summary>
/// Item detail queries
/// </summary>
public interface IItemDetailQuery
{
    /// <summary>
    /// Get item by ID
    /// </summary>
    Task<ItemResult> GetByIdAsync(string id, string tenantId);

    /// <summary>
    /// Get items by IDs
    /// </summary>
    Task<IEnumerable<ItemResult>> GetItemsByIdAsync(IEnumerable<string> ids, string tenantId);
}
