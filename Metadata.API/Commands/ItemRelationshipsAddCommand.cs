using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to add relationships from one item to others.
/// </summary>
public class ItemRelationshipsAddCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    [Required]
    public IEnumerable<RelationshipEntry> Relationships { get; init; } = Enumerable.Empty<RelationshipEntry>();
}
