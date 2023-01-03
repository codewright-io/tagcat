using System.ComponentModel.DataAnnotations;
using CodeWright.Common.EventSourcing;

namespace CodeWright.Metadata.API.Commands;

/// <summary>
/// Base class for commands
/// </summary>
public abstract class ItemCommandBase : IDomainCommand
{
    /// <summary>
    /// Unique identifier for the object
    /// </summary>
    [Required, StringLength(Identifiers.MaximumLength)]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Tenant Id for the object
    /// </summary>
    [Required, StringLength(Identifiers.MaximumLength)]
    public string TenantId { get; init; } = string.Empty;
}
