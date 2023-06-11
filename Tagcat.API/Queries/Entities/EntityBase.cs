using CodeWright.Common.EventSourcing;
using System.ComponentModel.DataAnnotations;

namespace CodeWright.Metadata.API.Queries.Entities;

/// <summary>
/// Base class for database entities
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Unique identifier for the object
    /// </summary>
    [StringLength(Identifiers.MaximumLength)]
    public string Id { get; set; } = String.Empty;

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    [StringLength(Identifiers.MaximumLength)]
    public string TenantId { get; set; } = String.Empty;

    /// <summary>
    /// Version of the view data
    /// </summary>
    public long Version { get; set; }
}
