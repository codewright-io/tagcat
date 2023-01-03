using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to set references between this item and others, replacing all existing references.
/// </summary>
public class ItemReferencesSetCommand : ItemCommandBase
{
    /// <summary>
    /// A list of references or relationships that the object has to other objects.
    /// </summary>
    public IEnumerable<ReferenceEntry> References { get; init; } = Enumerable.Empty<ReferenceEntry>();
}
