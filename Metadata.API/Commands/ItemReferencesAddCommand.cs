using System.ComponentModel.DataAnnotations;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to add references from one item to others.
/// </summary>
public class ItemReferencesAddCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    [Required]
    public IEnumerable<ReferenceEntry> References { get; init; } = Enumerable.Empty<ReferenceEntry>();
}
