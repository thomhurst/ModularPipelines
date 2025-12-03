using System.Collections;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers;

public static class CommandOptionsObjectArgumentParser
{
    public static void AddArgumentsFromOptionsObject(List<string> precedingArguments, object optionsArgumentsObject)
    {
        var properties = GetProperties(optionsArgumentsObject);

        // Check if using new CLI attribute system
        var cliArgumentProperties = properties.Where(p => p.GetCustomAttribute<CliArgumentAttribute>() is not null).ToList();
        var hasNewCliAttributes = cliArgumentProperties.Any()
            || properties.Any(p => p.GetCustomAttribute<CliFlagAttribute>() is not null)
            || properties.Any(p => p.GetCustomAttribute<CliOptionAttribute>() is not null);

        if (hasNewCliAttributes)
        {
            AddArgumentsUsingNewAttributes(precedingArguments, optionsArgumentsObject, properties, cliArgumentProperties);
            return;
        }

        // Legacy attribute handling
        var positionalArgumentProperties = properties.Where(p => p.GetCustomAttribute<PositionalArgumentAttribute>() is not null).ToList();

        AddPlaceholderArguments(precedingArguments, optionsArgumentsObject, positionalArgumentProperties);

        var positionalArgumentsBefore = positionalArgumentProperties.Where(p =>
        {
            var positionalArgumentAttribute = p.GetCustomAttribute<PositionalArgumentAttribute>()!;
            return positionalArgumentAttribute.PlaceholderName == null && positionalArgumentAttribute!.Position == Position.BeforeSwitches;
        });

        var positionalArgumentsAfter = positionalArgumentProperties
            .Where(p =>
            {
                var positionalArgumentAttribute = p.GetCustomAttribute<PositionalArgumentAttribute>()!;
                return positionalArgumentAttribute.PlaceholderName == null && positionalArgumentAttribute.Position == Position.AfterSwitches;
            });

        AddPositionalArguments(precedingArguments, optionsArgumentsObject, positionalArgumentsBefore);

        AddSwitches(precedingArguments, optionsArgumentsObject, properties);

        AddPositionalArguments(precedingArguments, optionsArgumentsObject, positionalArgumentsAfter);
    }

    private static void AddArgumentsUsingNewAttributes(
        List<string> args,
        object optionsArgumentsObject,
        PropertyInfo[] properties,
        List<PropertyInfo> cliArgumentProperties)
    {
        // Get arguments by placement
        var argumentsBeforeOptions = cliArgumentProperties
            .Where(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Placement == ArgumentPlacement.BeforeOptions)
            .OrderBy(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Position);

        var argumentsImmediatelyAfterCommand = cliArgumentProperties
            .Where(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Placement == ArgumentPlacement.ImmediatelyAfterCommand)
            .OrderBy(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Position);

        var argumentsAfterOptions = cliArgumentProperties
            .Where(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Placement == ArgumentPlacement.AfterOptions)
            .OrderBy(p => p.GetCustomAttribute<CliArgumentAttribute>()!.Position);

        // Add arguments immediately after command first
        AddCliArguments(args, optionsArgumentsObject, argumentsImmediatelyAfterCommand);

        // Add arguments before options
        AddCliArguments(args, optionsArgumentsObject, argumentsBeforeOptions);

        // Add flags and options
        AddCliFlagsAndOptions(args, optionsArgumentsObject, properties);

        // Add arguments after options
        AddCliArguments(args, optionsArgumentsObject, argumentsAfterOptions);
    }

    private static void AddCliArguments(List<string> args, object optionsArgumentsObject, IEnumerable<PropertyInfo> argumentProperties)
    {
        foreach (var propertyInfo in argumentProperties)
        {
            var propertyValues = GetValues(optionsArgumentsObject, propertyInfo)?.ToList();

            if (propertyValues is null || !propertyValues.Any())
            {
                continue;
            }

            args.AddRange(propertyValues);
        }
    }

    private static void AddCliFlagsAndOptions(List<string> args, object optionsArgumentsObject, PropertyInfo[] properties)
    {
        foreach (var propertyInfo in properties)
        {
            var propertyValues = GetValues(optionsArgumentsObject, propertyInfo)?.ToList();

            if (propertyValues is null || !propertyValues.Any())
            {
                continue;
            }

            // Try CliFlag first
            var cliFlagAttribute = propertyInfo.GetCustomAttribute<CliFlagAttribute>();
            if (cliFlagAttribute is not null)
            {
                if (bool.TryParse(propertyValues.First(), out var boolValue) && boolValue)
                {
                    args.Add(cliFlagAttribute.GetEffectiveName());
                }

                continue;
            }

            // Try CliOption
            var cliOptionAttribute = propertyInfo.GetCustomAttribute<CliOptionAttribute>();
            if (cliOptionAttribute is not null)
            {
                foreach (var propertyValue in propertyValues)
                {
                    if (string.IsNullOrWhiteSpace(propertyValue))
                    {
                        continue;
                    }

                    var optionName = cliOptionAttribute.GetEffectiveName();
                    var separator = cliOptionAttribute.GetSeparator();

                    if (separator == " ")
                    {
                        args.Add(optionName);
                        args.Add(propertyValue);
                    }
                    else
                    {
                        args.Add($"{optionName}{separator}{propertyValue}");
                    }
                }

                continue;
            }

            // Fall back to legacy attributes for mixed usage
            AddSwitchesForProperty(args, propertyValues, propertyInfo);
        }
    }

    private static void AddSwitchesForProperty(List<string> args, List<string> propertyValues, PropertyInfo propertyInfo)
    {
        foreach (var propertyValue in propertyValues)
        {
            if (TryAddBooleanSwitch(args, propertyInfo, propertyValue))
            {
                continue;
            }

            AddValueSwitch(args, propertyInfo, propertyValue);
        }
    }

    private static void AddPlaceholderArguments(List<string> precedingArguments, object optionsArgumentsObject,
        IEnumerable<PropertyInfo> positionalArgumentProperties)
    {
        var positionalPlaceholderArguments = positionalArgumentProperties
            .Where(p => p.GetCustomAttribute<PositionalArgumentAttribute>()!.PlaceholderName != null);

        foreach (var positionalPlaceholderArgument in positionalPlaceholderArguments)
        {
            var value = positionalPlaceholderArgument.GetValue(optionsArgumentsObject)?.ToString();
            var placeholderName = positionalPlaceholderArgument.GetCustomAttribute<PositionalArgumentAttribute>()!
                .PlaceholderName;

            var indexOfMatchingPrecedingArgumentPlaceholder =
                precedingArguments.FindIndex(x => x == placeholderName);

            if (indexOfMatchingPrecedingArgumentPlaceholder < 0 && !string.IsNullOrEmpty(value))
            {
                precedingArguments.Add(value);
                continue;
            }

            if (string.IsNullOrWhiteSpace(value) && precedingArguments[indexOfMatchingPrecedingArgumentPlaceholder].StartsWith('<'))
            {
                throw new ArgumentException($"No value provided for property {positionalPlaceholderArgument.Name}");
            }

            if (string.IsNullOrWhiteSpace(value) && indexOfMatchingPrecedingArgumentPlaceholder >= 0)
            {
                precedingArguments.RemoveAt(indexOfMatchingPrecedingArgumentPlaceholder);
                return;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            precedingArguments[indexOfMatchingPrecedingArgumentPlaceholder] = value;
        }
    }

    private static PropertyInfo[] GetProperties(object optionsArgumentsObject)
    {
        var type = optionsArgumentsObject.GetType();

        var publicProperties = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var nonPublicProperties = type
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);

        return publicProperties.Concat(nonPublicProperties).ToArray();
    }

    private static void AddSwitches(List<string> parsedArgs, object optionsArgumentsObject, PropertyInfo[] properties)
    {
        foreach (var propertyInfo in properties)
        {
            var propertyValues = GetValues(optionsArgumentsObject, propertyInfo)?.ToList();

            if (propertyValues is null || !propertyValues.Any())
            {
                continue;
            }

            AddSwitches(parsedArgs, propertyValues, propertyInfo);
        }
    }

    private static void AddPositionalArguments(List<string> parsedArgs, object optionsArgumentsObject, IEnumerable<PropertyInfo> positionalArgumentProperties)
    {
        foreach (var positionalArgumentPropertyInfo in positionalArgumentProperties)
        {
            var propertyValues = GetValues(optionsArgumentsObject, positionalArgumentPropertyInfo)?.ToList();

            if (propertyValues is null || !propertyValues.Any())
            {
                continue;
            }

            parsedArgs.AddRange(propertyValues);
        }
    }

    private static void AddSwitches(List<string> parsedArgs, List<string> propertyValues, PropertyInfo propertyInfo)
    {
        foreach (var propertyValue in propertyValues)
        {
            if (TryAddBooleanSwitch(parsedArgs, propertyInfo, propertyValue))
            {
                continue;
            }

            AddValueSwitch(parsedArgs, propertyInfo, propertyValue);
        }
    }

    private static void AddValueSwitch(ICollection<string> parsedArgs, MemberInfo propertyInfo, string propertyValue)
    {
        if (string.IsNullOrWhiteSpace(propertyValue))
        {
            return;
        }

        var commandSwitchAttribute = propertyInfo.GetCustomAttribute<CommandSwitchAttribute>(true);

        if (commandSwitchAttribute == null)
        {
            return;
        }

        if (commandSwitchAttribute.SwitchValueSeparator == " ")
        {
            parsedArgs.Add(commandSwitchAttribute.SwitchName);
            parsedArgs.Add(propertyValue);
        }
        else
        {
            parsedArgs.Add($"{commandSwitchAttribute.SwitchName}{commandSwitchAttribute.SwitchValueSeparator}{propertyValue}");
        }
    }

    private static bool TryAddBooleanSwitch(List<string> parsedArgs, PropertyInfo propertyInfo, string propertyValue)
    {
        var booleanCommandSwitchAttribute = propertyInfo.GetCustomAttribute<BooleanCommandSwitchAttribute>();

        if (booleanCommandSwitchAttribute is not null &&
            bool.TryParse(propertyValue, out var boolValue)
            && boolValue)
        {
            parsedArgs.Add(booleanCommandSwitchAttribute.SwitchName);
            return true;
        }

        return false;
    }

    private static IEnumerable<string>? GetValues(object optionsArgumentsObject, PropertyInfo propertyInfo)
    {
        var rawValue = propertyInfo.GetValue(optionsArgumentsObject);

        var singleValue = GetSingleValue(rawValue);

        if (singleValue is not null)
        {
            return [singleValue];
        }

        return GetValues(rawValue);
    }

    private static string? GetSingleValue(object? rawValue)
    {
        if (rawValue is null)
        {
            return null;
        }

        if (rawValue is string stringValue)
        {
            return stringValue;
        }

        if (rawValue is IEnumerable and not IEnumerable<char>)
        {
            return null;
        }

        if (rawValue is bool boolValue)
        {
            return boolValue.ToString().ToLowerInvariant();
        }

        if (rawValue
            is byte
            or short
            or ushort
            or int
            or uint
            or long
            or ulong
            or float
            or double
            or decimal)
        {
            return rawValue.ToString()!;
        }

        if (rawValue.GetType().IsEnum)
        {
            return ParseEnum(rawValue);
        }

        if (rawValue is Uri uri)
        {
            return ParseUri(uri);
        }

        return rawValue.ToString()!;
    }

    private static IEnumerable<string>? GetValues(object? rawValue)
    {
        if (rawValue is IEnumerable<KeyValue> keyValues)
        {
            return ParseKeyValues(keyValues);
        }

        if (rawValue is not IEnumerable enumerable)
        {
            return null;
        }

        if (rawValue is IEnumerable<char>)
        {
            return null;
        }

        var objects = enumerable.Cast<object>().ToArray();

        var list1 = objects
            .Select(GetSingleValue)
            .OfType<string>()
            .ToList();

        var list2 = objects
            .SelectMany(x => GetValues(x) ?? Array.Empty<string>())
            .ToList();

        return list1.Concat(list2);
    }

    private static IEnumerable<string>? ParseKeyValues(IEnumerable<KeyValue> keyValues)
    {
        return keyValues.Select(x => x.ToString());
    }

    private static string ParseEnum(object rawValue)
    {
        var enumValueAttribute = rawValue
            .GetType()
            .GetField(rawValue.ToString()!)
            ?.GetCustomAttribute<EnumValueAttribute>();

        return enumValueAttribute is not null
            ? enumValueAttribute.Value
            : rawValue.ToString()!;
    }

    private static string ParseUri(Uri uri)
    {
        return uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
    }
}