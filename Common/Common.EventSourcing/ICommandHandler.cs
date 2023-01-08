namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Common interface for classes that receive and process commands.
/// </summary>
/// <typeparam name="TCommand">The type of command to receive</typeparam>
public interface ICommandHandler<TCommand> 
    where TCommand : IDomainCommand, new()
{
    /// <summary>
    /// Process the command
    /// </summary>
    /// <param name="command">The command to process</param>
    /// <param name="userId">The user that is processing the command</param>
    /// <returns>The result</returns>
    Task<CommandResult> HandleAsync(TCommand command, string userId);
}
