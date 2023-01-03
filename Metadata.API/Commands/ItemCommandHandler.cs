using CodeWright.Common.EventSourcing;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands
{
    /// <summary>
    /// Handler for Item Commands
    /// </summary>
    public class ItemCommandHandler :
        ICommandHandler<ItemSetAllCommand>,
        ICommandHandler<ItemRemoveAllCommand>
    {
        private readonly ICommandHandler<ItemReferencesSetCommand> _referencesHandler;
        private readonly ICommandHandler<ItemMetadataSetCommand> _metadataHandler;
        private readonly ILogger _log;

        /// <summary>
        /// Create an instance of a ItemCommandHandler.
        /// </summary>
        public ItemCommandHandler(
            ICommandHandler<ItemReferencesSetCommand> referencesHandler,
            ICommandHandler<ItemMetadataSetCommand> metadataHandler,
            ILogger<ItemCommandHandler> log)
        {
            _referencesHandler = referencesHandler;
            _metadataHandler = metadataHandler;
            _log = log;
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemSetAllCommand command, string userId)
        {
            var referenceResult = await _referencesHandler.HandleAsync(new ItemReferencesSetCommand 
            { 
                TenantId = command.TenantId,
                Id = command.TenantId,
                References = command.References,
            }, userId);
            var metadataResult = await _metadataHandler.HandleAsync(new ItemMetadataSetCommand
            {
                TenantId = command.TenantId,
                Id = command.TenantId,
                Metadata = command.Metadata,
            }, userId);

            return new CommandResult { Version = long.Max(referenceResult.Version, metadataResult.Version) };
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
                References = Enumerable.Empty<ReferenceEntry>(),
            }, userId);
        }
    }
}
