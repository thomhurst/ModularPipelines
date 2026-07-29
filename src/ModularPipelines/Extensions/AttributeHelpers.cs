using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ModularPipelines.Extensions;

internal static class AttributeHelpers
{
    private static readonly ConcurrentDictionary<(Type Type, Type AttributeType), object[]> Cache = new();

    public static IEnumerable<T> GetCustomAttributesIncludingBaseInterfaces<T>(this Type type)
        where T : Attribute
    {
        var key = (type, typeof(T));

        if (Cache.TryGetValue(key, out var cachedResult))
        {
            return cachedResult.Cast<T>();
        }

        var attributes = GetAttributesInternal<T>(type);
        Cache[key] = attributes;
        return attributes.Cast<T>();
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "This method is the documented reflection fallback for dynamically supplied types; generated module event metadata handles statically known modules.")]
    private static object[] GetAttributesInternal<T>(Type type)
        where T : Attribute
    {
        var results = new List<object>();

        foreach (var result in type.GetInterfaces()
                     .SelectMany(i => i.GetCustomAttributes<T>(true)))
        {
            results.Add(result);
        }

        foreach (var customAttribute in type.GetCustomAttributes<T>(true))
        {
            results.Add(customAttribute);
        }

        return results.ToArray();
    }
}
