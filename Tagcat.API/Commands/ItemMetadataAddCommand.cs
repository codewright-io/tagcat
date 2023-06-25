using System.ComponentModel.DataAnnotations;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to add metadata to an item.
/// </summary>
public class ItemMetadataAddCommand : ItemCommandBase
{
    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    [Required]
    public required IEnumerable<MetadataEntry> Metadata { get; init; }
}
