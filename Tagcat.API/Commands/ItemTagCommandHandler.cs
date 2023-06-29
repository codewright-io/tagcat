using System.Globalization;
using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.Exceptions;
using CodeWright.Tagcat.API.Model;
using CodeWright.Tagcat.API.Queries.Interfaces;
using IdGen;

namespace CodeWright.Tagcat.API.Commands;

/// <summary>
/// Handler for Item Tag Commands
/// </summary>
public class ItemTagCommandHandler :
    ICommandHandler<ItemTagsAddCommand>,
    ICommandHandler<ItemTagsRemoveCommand>,
    ICommandHandler<ItemTagsSetCommand>
{
    private readonly IDomainRepository<Item> _repository;
    private readonly ITimeProvider _timeProvider;
    private readonly IVersionProvider _versionProvider;
    private readonly IItemMetadataQuery _metadataQuery;
    private readonly ICommandHandler<ItemMetadataAddCommand> _metadataAddHandler;
    private readonly IdGenerator _idGenerator;
    private readonly ServiceSettings _settings;
    private readonly ILogger _log;

    /// <summary>
    /// Create an instance of a ItemTagCommandHandler.
    /// </summary>
    public ItemTagCommandHandler(
        IDomainRepository<Item> repository,
        ITimeProvider timeProvider,
        IVersionProvider versionProvider,
        IItemMetadataQuery metadataQuery,
        ICommandHandler<ItemMetadataAddCommand> metadataAddHandler,
        IdGenerator idGenerator,
        ServiceSettings settings,
        ILogger<ItemTagCommandHandler> log)
    {
        _repository = repository;
        _timeProvider = timeProvider;
        _versionProvider = versionProvider;
        _metadataQuery = metadataQuery;
        _metadataAddHandler = metadataAddHandler;
        _idGenerator = idGenerator;
        _settings = settings;
        _log = log;
    }

    private async Task<IEnumerable<string>> CheckOrCreateTagItemsAsync(IEnumerable<string> tags, string tenantId, string culture, string userId)
    {
        // Check if the tags exist in the sustem, and if they don't, create them
        var tagIds = await tags
            .ToAsyncEnumerable()
            .SelectAwait(async (tag) =>
            {
                // Find a tag with the requested display name and culture
                var matchIds = await _metadataQuery.GetItemIdsByMetadataAsync(
                    tenantId, MetadataNames.Name, tag, MetadataNames.Culture, culture, 1, 0);

                var tagMetadata = new List<MetadataEntry>
                {
                    new MetadataEntry { Name = MetadataNames.Name, Value = tag },
                    new MetadataEntry { Name = MetadataNames.Culture, Value = culture },
                };
                return matchIds.FirstOrDefault() ?? await CreateTagAsync(tagMetadata, tenantId, userId);
            })
            .ToListAsync();

        return tagIds;
    }

    private async Task<string> CreateTagAsync(IEnumerable<MetadataEntry> metadata, string tenantId, string userId)
    {
        if (metadata == null) throw new ArgumentNullException(nameof(metadata));

        string tagId = _idGenerator.CreateId().ToString(CultureInfo.InvariantCulture);
        var addCommand = new ItemMetadataAddCommand { Id = tagId, TenantId = tenantId, Metadata = metadata };
        await _metadataAddHandler.HandleAsync(addCommand, userId);

        _log.LogInformation("Created tag {0} with id {1}", 
            metadata.FirstOrDefault(m => m.Name == MetadataNames.Name), 
            tagId);

        return tagId;
    }

    /// <summary>
    /// Handle the command
    /// </summary>
    public async Task<CommandResult> HandleAsync(ItemTagsAddCommand command, string userId)
    {
        if (string.IsNullOrEmpty(command.Culture))
            throw new InvalidInternalStateException("No culture was determined");

        var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
        if (item == null)
        {
            item = new Item { Id = command.Id, TenantId = command.TenantId };
            item.StartQueuing();
        }

        // Check if the tags exist in the system, and if they don't, create them
        var tagIds = await CheckOrCreateTagItemsAsync(command.Tags, command.TenantId, command.Culture, userId);

        // Add the tag as a relationships to the tag item
        var relationships = tagIds.Select(k => new RelationshipEntry { TargetId = k, Type = RelationshipType.Tag });
        item.AddRelationships(relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

        await _repository.SaveAsync(item, userId);
        return new CommandResult { Id = item.Id, Version = item.Version };
    }

    /// <summary>
    /// Handle the command
    /// </summary>
    public async Task<CommandResult> HandleAsync(ItemTagsRemoveCommand command, string userId)
    {
        if (string.IsNullOrEmpty(command.Culture))
            throw new InvalidInternalStateException("No culture was determined");

        var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
        if (item == null)
            throw new NotFoundException("Item not found");

        // Check if the tags exist in the system, and if they don't, create them
        var tagIds = await CheckOrCreateTagItemsAsync(command.Tags, command.TenantId, command.Culture, userId);

        var relationships = tagIds.Select(k => new RelationshipEntry { TargetId = k, Type = RelationshipType.Tag });
        item.RemoveRelationships(relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

        await _repository.SaveAsync(item, userId);
        return new CommandResult { Id = item.Id, Version = item.Version };
    }

    /// <summary>
    /// Handle the command
    /// </summary>
    public async Task<CommandResult> HandleAsync(ItemTagsSetCommand command, string userId)
    {
        if (string.IsNullOrEmpty(command.Culture))
            throw new InvalidInternalStateException("No culture was determined");

        var item = await _repository.GetByIdAsync(command.Id, command.TenantId, Item.DomainTypeId);
        if (item == null)
        {
            item = new Item { Id = command.Id, TenantId = command.TenantId };
            item.StartQueuing();
        }

        // Check if the tags exist in the system, and if they don't, create them
        var tagIds = await CheckOrCreateTagItemsAsync(command.Tags, command.TenantId, command.Culture, userId);

        var relationships = tagIds.Select(k => new RelationshipEntry { TargetId = k, Type = RelationshipType.Tag });
        item.SetRelationships(relationships, _versionProvider.GetNewVersion(), _timeProvider.GetCurrentTimeUtc(), userId, _settings.ServiceId);

        await _repository.SaveAsync(item, userId);
        return new CommandResult { Id = item.Id, Version = item.Version };
    }
}
