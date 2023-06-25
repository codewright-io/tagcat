using System.ComponentModel.DataAnnotations;

namespace CodeWright.Tagcat.API.Commands;

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
    public required IEnumerable<string> Tags { get; init; }
}
