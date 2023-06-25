using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to set relationships between this item and others, replacing all existing relationships.
/// </summary>
public class ItemRelationshipsSetCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public required IEnumerable<RelationshipEntry> Relationships { get; init; }
}
