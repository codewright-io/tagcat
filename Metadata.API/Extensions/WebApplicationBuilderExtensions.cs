namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// Extensions for WebApplicationBuilder
/// </summary>
public static class WebApplicationBuilderExtensions
{
    /// <summary>
    /// Create a logger for use while building the web application
    /// </summary>
    public static ILogger CreateLogger(this WebApplicationBuilder builder, string name)
    {
        using var loggerFactory = LoggerFactory.Create(config =>
        {
            config.AddConsole();
            config.AddConfiguration(builder.Configuration.GetSection("Logging"));
        });
        return loggerFactory.CreateLogger(name);
    }
}
