using System.ComponentModel.DataAnnotations;

namespace CodeWright.Metadata.API.Model;

/// <summary>
/// Reference information on an item
/// </summary>
public class ReferenceEntry : IEquatable<ReferenceEntry>
{
    /// <summary>
    /// The type of reference
    /// </summary>
    public ReferenceType Type { get; init; }

    /// <summary>
    /// The target ID of the referenced object
    /// </summary>
    [Required]
    public string TargetId { get; init; } = string.Empty;

    #region IEquatable
    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return Equals(obj as ReferenceEntry);
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Type.GetHashCode() ^ string.GetHashCode(TargetId, StringComparison.InvariantCulture);
    }

    /// <inheritdoc/>
    public bool Equals(ReferenceEntry? other)
    {
        if (other == null)
            return false;

        return other.Type == Type && other.TargetId == TargetId;
    }
    #endregion IEquatable
}
