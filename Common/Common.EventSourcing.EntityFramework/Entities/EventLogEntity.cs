using System.ComponentModel.DataAnnotations;

namespace CodeWright.Common.EventSourcing.EntityFramework.Entities;

public class EventLogEntity
{
    /// <summary>The domain object unique ID that the event was generated for</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string Id { get; set; } = string.Empty;

    /// <summary>Tenant Id for the object</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string TenantId { get; set; } = string.Empty;

    /// <summary>Version for event ordering</summary>
    public long Version { get; set; }

    /// <summary>The type information for the object</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string TypeId { get; set; } = string.Empty;

    /// <summary>The user that made this change</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string? UserId { get; set; }

    /// <summary>The ID of the service or entity that created this event</summary>
    [StringLength(Identifiers.MaximumLength)]
    public string SourceId { get; set; } = string.Empty;

    /// <summary>Time that the event was generated</summary>
    public DateTime CreateTime { get; set; }

    /// <summary>Serialized Event Data</summary>
    public string Content { get; set; } = string.Empty;
}
