using System.ComponentModel.DataAnnotations;
using CodeWright.Common.EventSourcing;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Base class for commands
/// </summary>
public abstract class ItemCommandBase : IDomainCommand
{
    /// <summary>
    /// Unique identifier for the object
    /// </summary>
    /// <example>A_Midsummer_Nights_Dream</example>
    [Required, StringLength(Identifiers.MaximumLength)]
    public required string Id { get; init; }

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    /// <example>William_Shakespeare</example>
    [Required, StringLength(Identifiers.MaximumLength)]
    public required string TenantId { get; init; }
}
