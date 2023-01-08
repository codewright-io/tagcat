using System.Reflection;
using System.Text.Json.Serialization;
using CodeWright.Common.EventSourcing;
using CodeWright.Common.EventSourcing.Snapshots;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Dependency injection extensions
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Register event sourcing services
    /// </summary>
    public static IServiceCollection AddEventSourcing(this IServiceCollection services)
    {
        // Add support for ID generation
        services.AddSingleton(p => new IdGen.IdGenerator(0));

        services.AddScoped<ITimeProvider, CurrentTimeProvider>();
        services.AddScoped<IVersionProvider, IdGenVersionProvider>();

        return services;
    }

    /// <summary>
    /// Register and event handler class with all its interfaces
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddWithEventHandlers<T>(this IServiceCollection services)
        where T : class
    {
        services.AddScoped<T>();
        var eventHandlers = typeof(T).GetEventHandlerInterfaces();
        foreach (var handler in eventHandlers)
        {
            services.AddScoped(handler, p => p.GetRequiredService<T>());
        }

        return services;
    }

    /// <summary>
    /// Register an internal event bus
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddInternalBus(this IServiceCollection services)
    {
        services.AddScoped<IEventBus, InternalEventBus>();

        return services;
    }
}
