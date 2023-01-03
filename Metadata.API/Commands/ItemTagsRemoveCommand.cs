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
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tags to remove.
    /// </summary>
    [Required]
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
