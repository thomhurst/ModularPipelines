using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class PlaceholderHandler : IPlaceholderHandler
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<PlaceholderPropertyInfo>> PlaceholderPropertyCache = new();

    /// <inheritdoc/>
    public List<string> ReplacePlaceholders(List<string> commandParts, object optionsObject)
    {
        if (commandParts.Count == 0)
        {
            return commandParts;
        }

        // Build a lookup of placeholder name -> property value
        var placeholderValues = BuildPlaceholderValueLookup(optionsObject);

        var result = new List<string>();
        foreach (var part in commandParts)
        {
            // Check if this is a placeholder (starts with < or [<)
            if (IsPlaceholder(part))
            {
                // Try to find a matching argument value
                if (placeholderValues.TryGetValue(part, out var values) && values.Count > 0)
                {
                    result.AddRange(values);
                }

                // If no value found, skip the placeholder (it's optional)
            }
            else
            {
                // Literal command part, add as-is
                result.Add(part);
            }
        }

        return result;
    }

    private static bool IsPlaceholder(string part)
    {
        return part.StartsWith('<') || part.StartsWith("[<");
    }

    private static Dictionary<string, List<string>> BuildPlaceholderValueLookup(object optionsObject)
    {
        var lookup = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var type = optionsObject.GetType();

        // Get cached placeholder properties for this type
        var placeholderProperties = PlaceholderPropertyCache.GetOrAdd(type, BuildPlaceholderPropertyCache);

        foreach (var placeholderProperty in placeholderProperties)
        {
            var rawValue = placeholderProperty.Getter(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            var values = GetArgumentValues(rawValue);
            if (values.Count > 0)
            {
                lookup[placeholderProperty.Name] = values;
            }
        }

        return lookup;
    }

    private static IReadOnlyList<PlaceholderPropertyInfo> BuildPlaceholderPropertyCache(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var placeholderProperties = new List<PlaceholderPropertyInfo>();

        foreach (var property in properties)
        {
            var cliArgument = property.GetCustomAttribute<CliArgumentAttribute>();
            if (cliArgument?.Name is null)
            {
                continue;
            }

            var getter = CreatePropertyGetter(type, property);
            placeholderProperties.Add(new PlaceholderPropertyInfo(cliArgument.Name, getter));
        }

        return placeholderProperties;
    }

    private static Func<object, object?> CreatePropertyGetter(Type type, PropertyInfo property)
    {
        // Create a compiled expression tree for fast property access
        // Expression: (object obj) => (object?)((TType)obj).Property
        var parameter = Expression.Parameter(typeof(object), "obj");
        var castToType = Expression.Convert(parameter, type);
        var propertyAccess = Expression.Property(castToType, property);
        var castToObject = Expression.Convert(propertyAccess, typeof(object));

        var lambda = Expression.Lambda<Func<object, object?>>(castToObject, parameter);
        return lambda.Compile();
    }

    private static List<string> GetArgumentValues(object rawValue)
    {
        var result = new List<string>();

        if (rawValue is string stringValue)
        {
            if (!string.IsNullOrEmpty(stringValue))
            {
                result.Add(stringValue);
            }
        }
        else if (rawValue is IEnumerable enumerable and not IEnumerable<char>)
        {
            foreach (var item in enumerable)
            {
                if (item is not null)
                {
                    var itemStr = item.ToString();
                    if (!string.IsNullOrEmpty(itemStr))
                    {
                        result.Add(itemStr);
                    }
                }
            }
        }
        else
        {
            var str = rawValue.ToString();
            if (!string.IsNullOrEmpty(str))
            {
                result.Add(str);
            }
        }

        return result;
    }

    /// <summary>
    /// Cached information about a property that provides placeholder values.
    /// </summary>
    /// <param name="Name">The placeholder name (from CliArgumentAttribute.Name).</param>
    /// <param name="Getter">A compiled delegate to get the property value.</param>
    private sealed record PlaceholderPropertyInfo(string Name, Func<object, object?> Getter);
}
