namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Command to set tags on an item, replacing all existing tags.
/// </summary>
public class ItemTagsSetCommand : ItemCommandBase
{
    /// <summary>
    /// Culture of the tags
    /// </summary>
    /// <example>en</example>
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tag data on the object.
    /// </summary>
    /// <example>["Comedy", "Romance"]</example>
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
