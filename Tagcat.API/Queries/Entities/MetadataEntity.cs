using System.ComponentModel.DataAnnotations;
using CodeWright.Common.EventSourcing;

namespace CodeWright.Tagcat.API.Queries.Entities;

/// <summary>
/// The database metadata table
/// </summary>
public class MetadataEntity : EntityBase
{
    /// <summary>
    /// The name of the metadata entry
    /// </summary>
    [Required, StringLength(Identifiers.MaximumLength)]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// The value of the metadata entry
    /// </summary>
    public string Value { get; init; } = string.Empty;
}
