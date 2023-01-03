namespace System.Reflection;

public static class TypeExtensions
{
    public static IEnumerable<Type> GetEventHandlerInterfaces(this Type type)
        => type.GetInterfaces().Where(i => i.Name.StartsWith("IEventHandler"));
}
