using System.ComponentModel.DataAnnotations;

namespace CodeWright.Common.EventSourcing;

/// <summary>
/// The result of a successful command applied to an object
/// </summary>
public class CommandResult
{
    /// <summary>
    /// Version of the object after the command was applied
    /// </summary>
    [Required]
    public long Version { get; init; }
}
