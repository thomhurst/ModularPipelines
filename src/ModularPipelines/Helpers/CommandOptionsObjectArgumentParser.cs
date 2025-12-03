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

        var cliArgumentProperties = properties.Where(p => p.GetCustomAttribute<CliArgumentAttribute>() is not null).ToList();

        AddArgumentsUsingNewAttributes(precedingArguments, optionsArgumentsObject, properties, cliArgumentProperties);
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