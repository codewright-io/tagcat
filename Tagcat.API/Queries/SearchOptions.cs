namespace CodeWright.Tagcat.API.Queries;

/// <summary>
/// Options for searches
/// </summary>
public sealed class SearchOptions
{
    /// <summary>
    /// Match on tag relationship
    /// </summary>
    public bool MatchOnRelationships { get; init; } = false;
}
