using System.ComponentModel.DataAnnotations;
using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Queries.Entities;

/// <summary>
/// The database reference table
/// </summary>
public class ReferenceEntity : EntityBase
{
    /// <summary>
    /// The type of reference
    /// </summary>
    public ReferenceType Type { get; init; }

    /// <summary>
    /// The target ID of the referenced object
    /// </summary>
    [Required, StringLength(Identifiers.MaximumLength)]
    public string TargetId { get; init; } = string.Empty;
}
