using CodeWright.Common.EventSourcing.EntityFramework;
using CodeWright.Tagcat.API.Queries;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// WebApplication extensions
/// </summary>
public static class WebApplicationExtensions
{
    /// <summary>
    /// Check that the databases are created.
    /// </summary>
    public static async Task EnsureTagDatabaseExistsAsync(this WebApplication application)
    {
        var scopeFactory = application.Services.GetRequiredService<IServiceScopeFactory>();
        using (var scope = scopeFactory.CreateScope())
        {
            var eventStoreContext = scope.ServiceProvider.GetRequiredService<EventSourceDbContext>();
            var metadataContext = scope.ServiceProvider.GetRequiredService<MetadataDbContext>();

            await eventStoreContext.Database.EnsureCreatedAsync();
            await metadataContext.Database.EnsureCreatedAsync();
        }
    }
}
