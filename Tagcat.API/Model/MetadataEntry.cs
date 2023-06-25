using System.ComponentModel.DataAnnotations;

namespace CodeWright.Tagcat.API.Model
{
    /// <summary>
    /// Metadata information on an item
    /// </summary>
    public class MetadataEntry : IEquatable<MetadataEntry>
    {
        /// <summary>
        /// The name or key for the metadata information
        /// </summary>
        /// <example>Year</example>
        [Required]
        public required string Name { get; init; }

        /// <summary>
        /// The value of the metadata information
        /// </summary>
        /// <example>1596</example>
        public required string Value { get; init; }

        #region IEquatable
        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return Equals(obj as MetadataEntry);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return string.GetHashCode(Name, StringComparison.InvariantCulture)
                ^ string.GetHashCode(Value, StringComparison.InvariantCulture);
        }

        /// <inheritdoc/>
        public bool Equals(MetadataEntry? other)
        {
            if (other == null)
                return false;

            return other.Name == Name && other.Value == Value;
        }
        #endregion IEquatable
    }
}
