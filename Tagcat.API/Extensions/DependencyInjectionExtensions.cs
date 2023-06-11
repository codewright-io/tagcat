using CodeWright.Common;
using CodeWright.Common.Asp;
using CodeWright.Common.EventSourcing;
using CodeWright.Tagcat.API.Commands;
using CodeWright.Tagcat.API.Commands.Validation;
using CodeWright.Tagcat.API.Events;
using CodeWright.Tagcat.API.Model;
using CodeWright.Tagcat.API.Queries;
using CodeWright.Tagcat.API.Queries.Interfaces;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service registration extensions
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Register all metadata service
    /// </summary>
    public static IServiceCollection AddAllMetadataService(this IServiceCollection services, ServiceSettings settings)
    {
        services.AddEntityFrameworkEventSourcing(settings.Database, settings.EventConnectionString);
        services.AddEvents<Item, ItemFactory>(typeof(ItemMetadataAddedEvent).Assembly);
        services.AddMetadataViewDatabase(settings.Database, settings.ViewConnectionString);
        services.AddMetadataCommands();
        services.AddMetadataQueries();
        services.AddMetadataViewUpdaters();

        return services;
    }

    /// <summary>
    /// Register command handlers and validators
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddMetadataCommands(this IServiceCollection services)
    {
        services.AddScoped<ItemCommandHandler>();

        services.AddScoped<ICommandHandler<ItemSetAllCommand>>(
           p => new ValidatingCommandHandler<ItemSetAllCommand, ItemSetAllCommandValidator>(p.GetRequiredService<ItemCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemRemoveAllCommand>>(
           p => new ValidatingCommandHandler<ItemRemoveAllCommand, ItemRemoveAllCommandValidator>(p.GetRequiredService<ItemCommandHandler>()));

        // Add metadata command handling
        services.AddScoped<ItemMetadataCommandHandler>();

        services.AddScoped<ICommandHandler<ItemMetadataAddCommand>>(
            p => new ValidatingCommandHandler<ItemMetadataAddCommand, ItemMetadataAddCommandValidator>(p.GetRequiredService<ItemMetadataCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemMetadataRemoveCommand>>(
            p => new ValidatingCommandHandler<ItemMetadataRemoveCommand, ItemMetadataRemoveCommandValidator>(p.GetRequiredService<ItemMetadataCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemMetadataSetCommand>>(
            p => new ValidatingCommandHandler<ItemMetadataSetCommand, ItemMetadataSetCommandValidator>(p.GetRequiredService<ItemMetadataCommandHandler>()));

        // Add relationship command handling
        services.AddScoped<ItemRelationshipsCommandHandler>();

        services.AddScoped<ICommandHandler<ItemRelationshipsSetCommand>>(
            p => new ValidatingCommandHandler<ItemRelationshipsSetCommand, ItemRelationshipSetCommandValidator>(p.GetRequiredService<ItemRelationshipsCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemRelationshipsAddCommand>>(
            p => new ValidatingCommandHandler<ItemRelationshipsAddCommand, ItemRelationshipsAddCommandValidator>(p.GetRequiredService<ItemRelationshipsCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemRelationshipsRemoveCommand>>(
            p => new ValidatingCommandHandler<ItemRelationshipsRemoveCommand, ItemRelationshipsRemoveCommandValidator>(p.GetRequiredService<ItemRelationshipsCommandHandler>()));

        // Add tag command handling
        services.AddScoped<ItemTagCommandHandler>();

        services.AddScoped<ICommandHandler<ItemTagsAddCommand>>(
            p => new ValidatingCommandHandler<ItemTagsAddCommand, ItemTagsAddCommandValidator>(p.GetRequiredService<ItemTagCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemTagsRemoveCommand>>(
            p => new ValidatingCommandHandler<ItemTagsRemoveCommand, ItemTagsRemoveCommandValidator>(p.GetRequiredService<ItemTagCommandHandler>()));
        services.AddScoped<ICommandHandler<ItemTagsSetCommand>>(
            p => new ValidatingCommandHandler<ItemTagsSetCommand, ItemTagsSetCommandValidator>(p.GetRequiredService<ItemTagCommandHandler>()));


        return services;
    }

    /// <summary>
    /// Register view updaters
    /// </summary>
    public static IServiceCollection AddMetadataViewUpdaters(this IServiceCollection services)
    {
        services.AddWithEventHandlers<ItemMetadataUpdater>();
        services.AddWithEventHandlers<ItemRelationshipUpdater>();

        return services;
    }

    /// <summary>
    /// Register command handlers and validators
    /// </summary>
    public static IServiceCollection AddMetadataViewDatabase(this IServiceCollection services, DatabaseType database, string connectionString)
    {
        services.AddDbContext<MetadataDbContext>(options => options.UseDatabase(database, connectionString, "CodeWright.Tagcat.API"));

        return services;
    }

    /// <summary>
    /// Register queries
    /// </summary>
    public static IServiceCollection AddMetadataQueries(this IServiceCollection services)
    {
        services.AddScoped<IItemTagQuery, ItemTagQuery>();
        services.AddScoped<IItemDetailQuery, ItemDetailQuery>();
        services.AddScoped<IItemMetadataQuery, ItemMetadataQuery>();
        services.AddScoped<IItemRelationshipQuery, ItemRelationshipQuery>();

        return services;
    }
}
