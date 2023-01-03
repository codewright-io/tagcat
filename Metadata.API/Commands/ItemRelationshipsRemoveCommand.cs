using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to remove existing relationships between this item and others.
/// </summary>
public class ItemRelationshipsRemoveCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships to remove.
    /// </summary>
    [Required]
    public IEnumerable<RelationshipEntry> Relationships { get; init; } = Enumerable.Empty<RelationshipEntry>();
}
