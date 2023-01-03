using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.Exceptions;
using CodeWright.Metadata.API.Model;

namespace CodeWright.Metadata.API.Commands
{
    /// <summary>
    /// Handler for Item Reference Commands
    /// </summary>
    public class ItemReferenceCommandHandler :
        ICommandHandler<ItemReferencesAddCommand>,
        ICommandHandler<ItemReferencesRemoveCommand>,
        ICommandHandler<ItemReferencesSetCommand>
    {
        private readonly IDomainRepository<Item> _repository;
        private readonly ITimeProvider _timeProvider;
        private readonly IVersionProvider _versionProvider;
        private readonly ServiceSettings _settings;
        private readonly ILogger _log;

        /// <summary>
        /// Create an instance of a ItemReferenceCommandHandler.
        /// </summary>
        public ItemReferenceCommandHandler(
            IDomainRepository<Item> repository,
            ITimeProvider timeProvider,
            IVersionProvider versionProvider,
            ServiceSettings settings,
            ILogger<ItemReferenceCommandHandler> log)
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
        public async Task<CommandResult> HandleAsync(ItemReferencesAddCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.AddReferences(command.References, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemReferencesRemoveCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
                throw new NotFoundException("Item not found");

            item.RemoveReferences(command.References, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }

        /// <summary>
        /// Handle the command
        /// </summary>
        public async Task<CommandResult> HandleAsync(ItemReferencesSetCommand command, string userId)
        {
            var item = await _repository.GetByIdAsync(command.Id, command.TenantId);
            if (item == null)
            {
                item = new Item { Id = command.Id, TenantId = command.TenantId };
                item.StartQueuing();
            }

            item.SetReferences(command.References, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

            await _repository.SaveAsync(item, userId);
            return new CommandResult { Version = item.Version };
        }
    }
}
