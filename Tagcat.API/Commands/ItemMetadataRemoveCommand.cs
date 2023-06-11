using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to remove existing metadata from an item.
/// </summary>
public class ItemMetadataRemoveCommand : ItemCommandBase
{
    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    [Required]
    public IEnumerable<MetadataEntry> Metadata { get; init; } = Enumerable.Empty<MetadataEntry>();
}
