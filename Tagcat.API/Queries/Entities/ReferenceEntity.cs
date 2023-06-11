using System.ComponentModel.DataAnnotations;
using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Queries.Entities;

/// <summary>
/// The database relationship table
/// </summary>
public class RelationshipEntity : EntityBase
{
    /// <summary>
    /// The type of relationship
    /// </summary>
    public RelationshipType Type { get; init; }

    /// <summary>
    /// The target ID of the relationship
    /// </summary>
    [Required, StringLength(Identifiers.MaximumLength)]
    public string TargetId { get; init; } = string.Empty;
}
