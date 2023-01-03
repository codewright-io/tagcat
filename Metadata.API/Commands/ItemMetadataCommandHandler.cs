using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.Exceptions;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands
{
    /// <summary>
    /// Handler for Item Metadata Commands
    /// </summary>
    public class ItemMetadataCommandHandler :
        ICommandHandler<ItemMetadataAddCommand>,
        ICommandHandler<ItemMetadataRemoveCommand>,
        ICommandHandler<ItemMetadataSetCommand>
    {
        private readonly IDomainRepository<Item> _repository;
        private readonly ITimeProvider _timeProvider;
        private readonly IVersionProvider _versionProvider;
        private readonly ServiceSettings _settings;
        private readonly ILogger _log;

        /// <summary>
        /// Create an instance of a ItemMetadataCommandHandler.
        /// </summary>
        public ItemMetadataCommandHandler(
            IDomainRepository<Item> repository,
            ITimeProvider timeProvider,
            IVersionProvider versionProvider,
            ServiceSettings settings,
            ILogger<ItemMetadataCommandHandler> log)
        {
            _repository = repository;
            _timeProvider = timeProvider;
            _versionProvider = versionProvider;
            _settings = settings;
            _log = log;
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemMetadataAddCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.AddMetadata(command.Metadata, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);

            // TODO send to bus?

            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemMetadataRemoveCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
                throw new NotFoundException("Item not found");

            item.RemoveMetadata(command.Metadata, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemMetadataSetCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.SetMetadata(command.Metadata, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }
    }
}
