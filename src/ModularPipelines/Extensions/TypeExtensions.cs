namespace ModularPipelines.Extensions;

internal static class TypeExtensions
{
    public static bool IsOrInheritsFrom(this Type type, Type otherType)
    {
        return type == otherType || type.IsSubclassOf(otherType);
    }
}