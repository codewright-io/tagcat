using System.ComponentModel.DataAnnotations;

namespace CodeWright.Common.EventSourcing.EntityFramework.Entities;

public class SnapshotEntity
{
    /// <summary>The domain object unique ID that the event was generated for</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Tenant Id for the object</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string TenantId { get; set; } = string.Empty;

    /// <summary>Version for event ordering</summary>
    public long Version { get; set; }

    /// <summary>Serialized Event Data</summary>
    public string Content { get; set; } = string.Empty;
}
