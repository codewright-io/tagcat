using System.ComponentModel.DataAnnotations;

namespace CodeWright.Tagcat.API.Model;

/// <summary>
/// Relationship information on an item
/// </summary>
public class RelationshipEntry : IEquatable<RelationshipEntry>
{
    /// <summary>
    /// The type of relationship
    /// </summary>
    /// <example>Tag</example>
    public RelationshipType Type { get; init; }

    /// <summary>
    /// The target ID of the relationship
    /// </summary>
    /// <example>Comedy</example>
    [Required]
    public string TargetId { get; init; } = string.Empty;

    #region IEquatable
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as RelationshipEntry);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Type.GetHashCode() ^ string.GetHashCode(TargetId, StringComparison.InvariantCulture);
    }

    /// <inheritdoc/>
    public bool Equals(RelationshipEntry? other)
    {
        if (other == null)
            return false;

        return other.Type == Type && other.TargetId == TargetId;
    }
    #endregion IEquatable
}
