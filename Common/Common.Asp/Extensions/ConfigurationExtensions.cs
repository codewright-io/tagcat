namespace Microsoft.Extensions.Configuration;

/// <summary>
/// Extention methods for configuration
/// </summary>
public static class ConfigurationExtensions
{
    public static string? GetString(this IConfiguration configuration, string section, string name, string environmentVariable)
    {
        string? stringProperty;
        if (!string.IsNullOrEmpty(environmentVariable))
        {
            stringProperty = Environment.GetEnvironmentVariable(environmentVariable);
            if (!string.IsNullOrWhiteSpace(stringProperty))
                return stringProperty;
        }

        stringProperty = configuration.GetSection($"{section}:{name}")?.Value;
        if (!string.IsNullOrWhiteSpace(stringProperty))
            return stringProperty;

        return null;
    }

    public static T? GetEnum<T>(this IConfiguration configuration, string section, string name, string environmentVariable)
        where T : struct, Enum
    {
        string? data = configuration.GetString(section, name, environmentVariable);
        if (string.IsNullOrWhiteSpace(data))
            return null;

        return Enum.TryParse<T>(data, out var result) ? result : null;
    }

    public static bool? GetBool(this IConfiguration configuration, string section, string name, string environmentVariable)
    {
        string? data = configuration.GetString(section, name, environmentVariable);
        if (string.IsNullOrWhiteSpace(data))
            return false;

        return bool.TryParse(data, out var result) ? result : null;
    }
}
