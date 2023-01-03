namespace CodeWright.Common.EventSourcing;

public interface IDomainCommand
{
    /// <summary>
    /// Unique identifier for the object (unique within the tenancy)
    /// </summary>
    string Id { get; init; }

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    string TenantId { get; init; }
}
