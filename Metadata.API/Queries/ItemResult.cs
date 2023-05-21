using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Queries;

/// <summary>
/// Item result for search queries
/// </summary>
public class ItemResult
{
    /// <summary>
    /// Unique identifier for the object
    /// </summary>
    /// <example>A_Midsummer_Nights_Dream</example>
    [Required]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    /// <example>William_Shakespeare</example>
    [Required]
    public string TenantId { get; init; } = string.Empty;

    /// <summary>
    /// Version of the object
    /// </summary>
    /// <example>123456</example>
    [Required]
    public long Version { get; init; }

    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<RelationshipEntry> Relationships { get; init; } = Enumerable.Empty<RelationshipEntry>();

    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    public IEnumerable<MetadataEntry> Metadata { get; init; } = Enumerable.Empty<MetadataEntry>();
}
