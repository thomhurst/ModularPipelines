using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ModularPipelines.Extensions;

internal static class TypeExtensions
{
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Selector interfaces are referenced by dependency attributes, preserving their contracts on registered module types.")]
    public static bool IsOrInheritsFrom(this Type type, Type otherType)
    {
        if (otherType.IsAssignableFrom(type))
        {
            return true;
        }

        // Handle open generic types (e.g., typeof(Step1Module<>))
        if (otherType.IsGenericTypeDefinition)
        {
            if (otherType.IsInterface)
            {
                foreach (var implementedInterface in type.GetInterfaces())
                {
                    if (implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == otherType)
                    {
                        return true;
                    }
                }

                return false;
            }

            return type.IsSubclassOfOpenGeneric(otherType);
        }

        return false;
    }

    private static bool IsSubclassOfOpenGeneric(this Type type, Type openGenericType)
    {
        var current = type;

        while (current != null && current != typeof(object))
        {
            if (current.IsGenericType && current.GetGenericTypeDefinition() == openGenericType)
            {
                return true;
            }

            current = current.BaseType;
        }

        return false;
    }

    public static string GetRealTypeName(this Type type)
    {
        if (!type.IsGenericType)
        {
            return type.Name;
        }

        var stringBuilder = new StringBuilder();

        stringBuilder.Append(type.Name.AsSpan(0, type.Name.IndexOf('`')));
        stringBuilder.Append('<');

        var appendComma = false;

        foreach (var genericTypeArgument in type.GetGenericArguments())
        {
            if (appendComma)
            {
                stringBuilder.Append(',');
            }

            stringBuilder.Append(GetRealTypeName(genericTypeArgument));
            appendComma = true;
        }

        stringBuilder.Append('>');

        return stringBuilder.ToString();
    }
}
