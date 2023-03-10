using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to set relationships between this item and others, replacing all existing relationships.
/// </summary>
public class ItemRelationshipsSetCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<RelationshipEntry> Relationships { get; init; } = Enumerable.Empty<RelationshipEntry>();
}
