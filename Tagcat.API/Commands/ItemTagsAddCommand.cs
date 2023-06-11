using System.ComponentModel.DataAnnotations;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to add tags to an item.
/// </summary>
public class ItemTagsAddCommand : ItemCommandBase
{
    /// <summary>
    /// Culture of the tags
    /// </summary>
    /// <example>en</example>
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tags to add.
    /// </summary>
    /// <example>["Comedy", "Romance"]</example>
    [Required]
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
