using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to set all metadata and relationships on an item, replacing the existing metadata and relationships.
/// </summary>
public class ItemSetAllCommand : ItemCommandBase
{
    /// <summary>
    /// A key/value list of metadata properties on the object.
    /// </summary>
    public required IEnumerable<MetadataEntry> Metadata { get; init; }

    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public required IEnumerable<RelationshipEntry> Relationships { get; init; }
}
