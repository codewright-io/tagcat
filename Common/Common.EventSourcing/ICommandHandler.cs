namespace CodeWright.Common.EventSourcing;

/// <summary>
/// Common interface for classes that receive and process commands.
/// </summary>
/// <typeparam name="TCommand">The type of command to receive</typeparam>
public interface ICommandHandler<TCommand> 
    where TCommand : IDomainCommand, new()
{
    Task<CommandResult> HandleAsync(TCommand command, string userId);
}
