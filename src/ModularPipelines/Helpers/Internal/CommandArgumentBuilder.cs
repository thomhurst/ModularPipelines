using System.Collections;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandArgumentBuilder : ICommandArgumentBuilder
{
    /// <inheritdoc/>
    public List<string> BuildArguments(IReadOnlyList<PropertyCommandLinePart> commandModel, object optionsObject)
    {
        var args = new List<string>();

        // Group arguments by placement
        var argumentsByPlacement = commandModel
            .OfType<ArgumentPart>()
            .GroupBy(a => a.Attribute.Placement)
            .ToDictionary(g => g.Key, g => g.OrderBy(a => a.Attribute.Position).ToList());

        var flagsAndOptions = commandModel.Where(p => p is FlagPart or OptionPart).ToList();

        // Add arguments immediately after command first
        AddArguments(args, argumentsByPlacement.GetValueOrDefault(ArgumentPlacement.ImmediatelyAfterCommand), optionsObject);

        // Add arguments before options
        AddArguments(args, argumentsByPlacement.GetValueOrDefault(ArgumentPlacement.BeforeOptions), optionsObject);

        // Add flags and options
        AddFlagsAndOptions(args, flagsAndOptions, optionsObject);

        // Add arguments after options
        AddArguments(args, argumentsByPlacement.GetValueOrDefault(ArgumentPlacement.AfterOptions), optionsObject);

        return args;
    }

    private static void AddArguments(List<string> args, List<ArgumentPart>? argumentParts, object optionsObject)
    {
        if (argumentParts is null)
        {
            return;
        }

        foreach (var argumentPart in argumentParts)
        {
            // Skip arguments that have a Name property - these are handled inline via
            // placeholder replacement in Command.cs and should not be added again
            if (argumentPart.Attribute.Name is not null)
            {
                continue;
            }

            var rawValue = argumentPart.Property.GetValue(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            var values = GetValues(rawValue);
            args.AddRange(values);
        }
    }

    private static void AddFlagsAndOptions(List<string> args, List<PropertyCommandLinePart> parts, object optionsObject)
    {
        foreach (var part in parts)
        {
            var rawValue = part.Property.GetValue(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            switch (part)
            {
                case FlagPart flagPart:
                    AddFlag(args, flagPart, rawValue);
                    break;
                case OptionPart optionPart:
                    AddOption(args, optionPart, rawValue);
                    break;
            }
        }
    }

    private static void AddFlag(List<string> args, FlagPart flagPart, object rawValue)
    {
        if (rawValue is bool boolValue && boolValue)
        {
            args.Add(flagPart.Attribute.GetEffectiveName());
        }
    }

    private static void AddOption(List<string> args, OptionPart optionPart, object rawValue)
    {
        var values = GetValues(rawValue);
        var addedCount = 0;

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            var optionName = optionPart.Attribute.GetEffectiveName();
            var separator = optionPart.Attribute.GetSeparator();

            if (separator == " ")
            {
                args.Add(optionName);
                args.Add(value);
            }
            else
            {
                args.Add($"{optionName}{separator}{value}");
            }

            addedCount++;

            // If AllowMultiple is false, only add the first value
            if (!optionPart.Attribute.AllowMultiple)
            {
                break;
            }
        }
    }

    private static List<string> GetValues(object rawValue)
    {
        var result = new List<string>();

        // Try single value first
        var singleValue = GetSingleValue(rawValue);
        if (singleValue is not null)
        {
            result.Add(singleValue);
            return result;
        }

        // Handle collections
        return GetCollectionValues(rawValue);
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

        // Collections are handled separately
        if (rawValue is IEnumerable and not IEnumerable<char>)
        {
            return null;
        }

        if (rawValue is bool boolValue)
        {
            return boolValue.ToString().ToLowerInvariant();
        }

        if (rawValue is byte or short or ushort or int or uint or long or ulong or float or double or decimal)
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

    private static List<string> GetCollectionValues(object? rawValue)
    {
        var result = new List<string>();

        if (rawValue is IEnumerable<KeyValue> keyValues)
        {
            result.AddRange(keyValues.Select(x => x.ToString()));
            return result;
        }

        if (rawValue is not IEnumerable enumerable)
        {
            return result;
        }

        if (rawValue is IEnumerable<char>)
        {
            return result;
        }

        foreach (var item in enumerable)
        {
            if (item is null)
            {
                continue;
            }

            var singleValue = GetSingleValue(item);
            if (singleValue is not null)
            {
                result.Add(singleValue);
            }
            else
            {
                // Recursively handle nested collections
                result.AddRange(GetCollectionValues(item));
            }
        }

        return result;
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
