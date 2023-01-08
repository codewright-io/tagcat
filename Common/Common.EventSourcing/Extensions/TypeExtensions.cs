namespace System.Reflection;

/// <summary>
/// Type reflection extension methods
/// </summary>
public static class TypeExtensions
{
    /// <summary>
    /// Get types that implement IEventHandler
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<Type> GetEventHandlerInterfaces(this Type type)
        => type.GetInterfaces().Where(i => i.Name.StartsWith("IEventHandler"));
}
