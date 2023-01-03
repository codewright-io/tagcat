using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to remove existing references between this item and others.
/// </summary>
public class ItemReferencesRemoveCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships to remove.
    /// </summary>
    [Required]
    public IEnumerable<ReferenceEntry> References { get; init; } = Enumerable.Empty<ReferenceEntry>();
}
