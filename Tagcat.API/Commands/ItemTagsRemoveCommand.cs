using System.ComponentModel.DataAnnotations;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to remove existing tags from an item.
/// </summary>
public class ItemTagsRemoveCommand : ItemCommandBase
{
    /// <summary>
    /// Culture of the tags
    /// </summary>
    /// <example>en</example>
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tags to remove.
    /// </summary>
    /// <example>["Comedy", "Romance"]</example>
    [Required]
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
