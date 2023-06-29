using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Handler for Item Commands
/// </summary>
public class ItemCommandHandler :
    ICommandHandler<ItemSetAllCommand>,
    ICommandHandler<ItemRemoveAllCommand>
{
    private readonly ICommandHandler<ItemRelationshipsSetCommand> _relationshipsHandler;
    private readonly ICommandHandler<ItemMetadataSetCommand> _metadataHandler;
    private readonly ILogger _log;

    /// <summary>
    /// Create an instance of a ItemCommandHandler.
    /// </summary>
    public ItemCommandHandler(
        ICommandHandler<ItemRelationshipsSetCommand> relationshipsHandler,
        ICommandHandler<ItemMetadataSetCommand> metadataHandler,
        ILogger<ItemCommandHandler> log)
    {
        _relationshipsHandler = relationshipsHandler;
        _metadataHandler = metadataHandler;
        _log = log;
    }

    /// <summary>
    /// Handle the command
    /// </summary>
    public async Task<CommandResult> HandleAsync(ItemSetAllCommand command, string userId)
    {
        var relationshipsResult = await _relationshipsHandler.HandleAsync(new ItemRelationshipsSetCommand 
        { 
            TenantId = command.TenantId,
            Id = command.TenantId,
            Relationships = command.Relationships,
        }, userId);
        var metadataResult = await _metadataHandler.HandleAsync(new ItemMetadataSetCommand
        {
            TenantId = command.TenantId,
            Id = command.TenantId,
            Metadata = command.Metadata,
        }, userId);

        return new CommandResult { Id = metadataResult.Id, Version = long.Max(relationshipsResult.Version, metadataResult.Version) };
    }

    /// <summary>
    /// Handle the command
    /// </summary>
    public Task<CommandResult> HandleAsync(ItemRemoveAllCommand command, string userId)
    {
        return HandleAsync(new ItemSetAllCommand
        {
            TenantId = command.TenantId,
            Id = command.TenantId,
            Metadata = Enumerable.Empty<MetadataEntry>(),
            Relationships = Enumerable.Empty<RelationshipEntry>(),
        }, userId);
    }
}
