using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to set all metadata and references on an item, replacing the existing metadata and references.
/// </summary>
public class ItemSetAllCommand : ItemCommandBase
{
    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    public IEnumerable<MetadataEntry> Metadata { get; init; } = Enumerable.Empty<MetadataEntry>();

    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<ReferenceEntry> References { get; init; } = Enumerable.Empty<ReferenceEntry>();
}
