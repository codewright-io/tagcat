using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service registration extensions
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Replace the existing DB context with a new one
    /// </summary>
    public static IServiceCollection ReplaceDbContext<TContext>(this IServiceCollection services, string connectionString)
        where TContext : DbContext
    {
        var contextService = services.First(s => s.ServiceType == typeof(TContext));
        services.Remove(contextService);

        // Need to also remove the db options or they will be re-used
        var optionsService = services.First(s => s.ServiceType == typeof(DbContextOptions<TContext>));
        services.Remove(optionsService);

        services.AddDbContext<TContext>(options => options.UseSqlite(connectionString));

        return services;
    }
}
