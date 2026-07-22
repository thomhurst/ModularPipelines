using System.Collections;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class PlaceholderHandler : IPlaceholderHandler
{
    private readonly ICommandModelProvider _commandModelProvider;

    public PlaceholderHandler(ICommandModelProvider commandModelProvider)
    {
        _commandModelProvider = commandModelProvider;
    }

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

    private Dictionary<string, List<string>> BuildPlaceholderValueLookup(object optionsObject)
    {
        var lookup = new Dictionary<string, List<string>>(StringComparer.Ordinal);
        var placeholderProperties = _commandModelProvider.GetCommandModel(optionsObject.GetType())
            .OfType<ArgumentPart>()
            .Where(argument => argument.Attribute.Name is not null);

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
                lookup[placeholderProperty.Attribute.Name!] = values;
            }
        }

        return lookup;
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
}
