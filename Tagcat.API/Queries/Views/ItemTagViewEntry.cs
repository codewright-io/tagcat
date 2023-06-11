namespace CodeWright.Tagcat.API.Queries.Views;

/// <summary>
/// An item tag entry
/// </summary>
public class ItemTagViewEntry
{
    /// <summary>
    /// The Tag ID
    /// </summary>
    public string Id { get; init; } = String.Empty;

    /// <summary>
    /// The tag display name
    /// </summary>
    public string DisplayName { get; init; } = String.Empty;
}
