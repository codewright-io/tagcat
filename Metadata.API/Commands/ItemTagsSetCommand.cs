namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Command to set tags on an item, replacing all existing tags.
/// </summary>
public class ItemTagsSetCommand : ItemCommandBase
{
    /// <summary>
    /// Culture of the tags
    /// </summary>
    public string? Culture { get; internal set; } = null;

    /// <summary>
    /// A list of tag data on the object.
    /// </summary>
    public IEnumerable<string> Tags { get; init; } = Enumerable.Empty<string>();
}
