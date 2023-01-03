using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CodeWright.Common.Asp;

/// <summary>
/// Basic settings for services
/// </summary>
public class ServiceSettings
{
    /// <summary>
    /// Create an instance of a ServiceSettings
    /// </summary>
    public ServiceSettings() { }

    /// <summary>
    /// Create an instance of a ServiceSettings
    /// </summary>
    public ServiceSettings(IConfiguration configuration)
    {
        ServiceId = configuration.GetString("Settings", "ServiceId", "SERVICE_ID") ?? Guid.NewGuid().ToString();
        Database = configuration.GetEnum<DatabaseType>("Settings", "Database", "DATABASE") ?? DatabaseType.SQLite;

        EventConnectionString = configuration.GetString("ConnectionStrings", "DefaultConnection", "DATABASE_CONNECTION") ?? string.Empty;
        ViewConnectionString = configuration.GetString("ConnectionStrings", "ViewConnection", "DATABASE_VIEW_CONNECTION") ?? EventConnectionString;

        ExposeSwaggerEndpoints = configuration.GetBool("Settings", "ExposeSwaggerEndpoints", "EXPOSE_SWAGGER") ?? false;
    }

    /// <summary>
    /// The unique ID for the service
    /// </summary>
    public string ServiceId { get; init; } = String.Empty;

    /// <summary>
    /// The database type for persistent data
    /// </summary>
    public DatabaseType Database { get; init; } = DatabaseType.SQLite;

    /// <summary>
    /// The database connection string for the event store
    /// </summary>
    public string EventConnectionString { get; init; } = String.Empty;

    /// <summary>
    /// The database connection string for the view database
    /// </summary>
    public string ViewConnectionString { get; init; } = String.Empty;

    /// <summary>
    /// True to expose the swagger endpoints
    /// </summary>
    public bool ExposeSwaggerEndpoints { get; init; }

    /// <summary>
    /// Log settings
    /// </summary>
    public void LogSettings(ILogger logger)
    {
        logger.LogInformation("Startup - Service ID: {serviceId}", ServiceId);
        logger.LogInformation("Startup - Using Database Provider: {database}", Database);
        logger.LogInformation("Startup - Event Connection String: {connection}", EventConnectionString.OfuscatePasswords());
        logger.LogInformation("Startup - View Connection String: {connection}", ViewConnectionString.OfuscatePasswords());
        logger.LogInformation("Startup - Expose swagger endpoints: {exposeSwaggerEndpoints}", ExposeSwaggerEndpoints);
    }
}
