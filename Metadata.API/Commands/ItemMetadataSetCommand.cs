using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to set metadata on an item, replacing existing metadata.
/// </summary>
public class ItemMetadataSetCommand : ItemCommandBase
{
    /// <summary>
    /// A list of metadata properties on the item.
    /// </summary>
    public IEnumerable<MetadataEntry> Metadata { get; init; } = Enumerable.Empty<MetadataEntry>();
}
