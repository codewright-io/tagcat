using CodeWright.Common.EventSourcing.EntityFramework;
using CodeWright.Tagcat.API.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeWright.Tagcat.Installer;

public static class Install
{
    public static async Task MigrateAsync(IServiceScopeFactory scopeFactory)
    {
        using var scope = scopeFactory.CreateScope();

        var provider = scope.ServiceProvider;

        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("Install");
        var eventSourceContext = provider.GetRequiredService<EventSourceDbContext>();
        var viewContext = provider.GetRequiredService<MetadataDbContext>();

        logger.LogInformation(" Creating Event Sourcing Tables");
        eventSourceContext.Database.SetCommandTimeout(300);
        await eventSourceContext.Database.MigrateAsync();

        logger.LogInformation("Creating View Tables");
        viewContext.Database.SetCommandTimeout(300);
        await viewContext.Database.MigrateAsync();
    }
}
