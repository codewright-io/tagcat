using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.Exceptions;
using CodeWright.Tagcat.API.Model;

namespace CodeWright.Tagcat.API.Commands
{
    /// <summary>
    /// Handler for Item Relationship Commands
    /// </summary>
    public class ItemRelationshipsCommandHandler :
        ICommandHandler<ItemRelationshipsAddCommand>,
        ICommandHandler<ItemRelationshipsRemoveCommand>,
        ICommandHandler<ItemRelationshipsSetCommand>
    {
        private readonly IDomainRepository<Item> _repository;
        private readonly ITimeProvider _timeProvider;
        private readonly IVersionProvider _versionProvider;
        private readonly ServiceSettings _settings;
        private readonly ILogger _log;

        /// <summary>
        /// Create an instance of a ItemRelationshipsCommandHandler.
        /// </summary>
        public ItemRelationshipsCommandHandler(
            IDomainRepository<Item> repository,
            ITimeProvider timeProvider,
            IVersionProvider versionProvider,
            ServiceSettings settings,
            ILogger<ItemRelationshipsCommandHandler> log)
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
        public async Task<CommandResult> HandleAsync(ItemRelationshipsAddCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.AddRelationships(command.Relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemRelationshipsRemoveCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
            if (item == null)
                throw new NotFoundException("Item not found");

            item.RemoveRelationships(command.Relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemRelationshipsSetCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.SetRelationships(command.Relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }
    }
}
