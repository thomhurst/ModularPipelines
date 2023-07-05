using System.Reflection;

namespace ModularPipelines.Extensions;

internal static class AttributeHelpers
{
    public static IEnumerable<T> GetCustomAttributesIncludingBaseInterfaces<T>(this Type type) where T : Attribute
    {
        foreach (var result in type.GetInterfaces()
                     .SelectMany(i => i.GetCustomAttributes<T>(true)))
        {
            yield return result;
        }

        foreach (var customAttribute in type.GetCustomAttributes<T>(true))
        {
            yield return customAttribute;
        }
    }
}