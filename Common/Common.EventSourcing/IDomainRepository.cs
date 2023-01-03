namespace CodeWright.Common.EventSourcing;

public interface IDomainRepository<T>
    where T : IDomainObject
{
    /// <summary>
    /// Get the domain entity
    /// </summary>
    Task<T?> GetByIdAsync(string id, string tenantId);

    /// <summary>
    /// Save the domain entity
    /// </summary>
    Task SaveAsync(T item, string userId);
}
