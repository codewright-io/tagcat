using System.ComponentModel.DataAnnotations;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to add tags to an item.
/// </summary>
public class ItemTagsAddCommand : ItemCommandBase
{
    /// <summary>
    /// Culture of the tags
    /// </summary>
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tags to add.
    /// </summary>
    [Required]
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
