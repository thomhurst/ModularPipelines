using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandArgumentBuilder : ICommandArgumentBuilder
{
    /// <inheritdoc/>
    public IReadOnlyList<string> BuildArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        object optionsObject)
    {
        var emittedOptionTerminator = false;
        return BuildArguments(commandModel, optionsObject, ref emittedOptionTerminator);
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> BuildArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        object optionsObject,
        ref bool emittedOptionTerminator)
    {
        var arguments = commandModel.OfType<ArgumentPart>().ToList();
        var flagsAndOptions = commandModel.Where(p => p is FlagPart or OptionPart).ToList();
        var renderedPhases = new Dictionary<CommandLinePhase, IReadOnlyList<string>>();
        foreach (var phase in Enum.GetValues<CommandLinePhase>())
        {
            renderedPhases.Add(
                phase,
                RenderPhase(
                    phase,
                    flagsAndOptions,
                    arguments,
                    optionsObject,
                    ref emittedOptionTerminator));
        }

        return renderedPhases
            .OrderBy(pair => GetRenderOrder(pair.Key))
            .SelectMany(pair => pair.Value)
            .ToList();
    }

    private static int GetRenderOrder(CommandLinePhase phase) => phase switch
    {
        CommandLinePhase.EarlyOperand => 0,
        CommandLinePhase.Normal => 1,
        CommandLinePhase.Passthrough => 2,
        CommandLinePhase.Terminal => 3,
        _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null),
    };

    private static List<string> RenderPhase(
        CommandLinePhase phase,
        IEnumerable<PropertyCommandLinePart> flagsAndOptions,
        IEnumerable<ArgumentPart> arguments,
        object optionsObject,
        ref bool emittedOptionTerminator)
    {
        var rendered = new List<string>();
        var phaseOptions = flagsAndOptions.Where(part => part.Phase == phase);
        var phaseArguments = arguments
            .Where(part => part.Phase == phase)
            .OrderBy(part => part.Attribute.Position);

        if (phase == CommandLinePhase.Terminal)
        {
            AddArguments(rendered, phaseArguments, optionsObject, ref emittedOptionTerminator);
            AddFlagsAndOptions(rendered, phaseOptions, optionsObject);
        }
        else
        {
            AddFlagsAndOptions(rendered, phaseOptions, optionsObject);
            AddArguments(rendered, phaseArguments, optionsObject, ref emittedOptionTerminator);
        }

        return rendered;
    }

    private static void AddArguments(
        List<string> args,
        IEnumerable<ArgumentPart>? argumentParts,
        object optionsObject,
        ref bool emittedOptionTerminator)
    {
        if (argumentParts is null)
        {
            return;
        }

        foreach (var argumentPart in argumentParts)
        {
            var rawValue = argumentPart.Getter(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            var values = GetValues(rawValue);
            if (argumentPart.Attribute.PrependOptionTerminator
                && values.Count > 0
                && !emittedOptionTerminator)
            {
                args.Add("--");
                emittedOptionTerminator = true;
            }

            args.AddRange(values);
        }
    }

    private static void AddFlagsAndOptions(
        List<string> args,
        IEnumerable<PropertyCommandLinePart> parts,
        object optionsObject)
    {
        foreach (var part in parts)
        {
            var rawValue = part.Getter(optionsObject);
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

        if (rawValue is int count && count > 0)
        {
            args.AddRange(Enumerable.Repeat(flagPart.Attribute.GetEffectiveName(), count));
        }
    }

    private static void AddOption(List<string> args, OptionPart optionPart, object rawValue)
    {
        var valuePairs = GetOptionValuePairs(rawValue);
        if (optionPart.Attribute.GroupValues)
        {
            var values = valuePairs is null
                ? GetValues(rawValue)
                : valuePairs.SelectMany(static pair => new[] { pair.First, pair.Second });
            AddGroupedOption(args, optionPart.Attribute, values);
            return;
        }

        if (valuePairs is not null)
        {
            AddOptionValuePairs(args, optionPart, valuePairs);
            return;
        }

        foreach (var value in GetValues(rawValue))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                if (optionPart.Attribute.ValueArity == CliOptionValueArity.Optional)
                {
                    args.Add(optionPart.Attribute.GetEffectiveName());
                }

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
        }
    }

    private static void AddGroupedOption(
        List<string> args,
        CliOptionAttribute attribute,
        IEnumerable<string> values)
    {
        if (attribute.GetSeparator() != " ")
        {
            throw new InvalidOperationException(
                $"Grouped option '{attribute.GetEffectiveName()}' must use a space separator.");
        }

        var renderedValues = values.Where(value => !string.IsNullOrWhiteSpace(value)).ToList();
        if (renderedValues.Count == 0)
        {
            if (attribute.ValueArity == CliOptionValueArity.Optional)
            {
                args.Add(attribute.GetEffectiveName());
            }

            return;
        }

        args.Add(attribute.GetEffectiveName());
        args.AddRange(renderedValues);
    }

    private static IEnumerable<CliValuePair>? GetOptionValuePairs(object rawValue)
    {
        return rawValue switch
        {
            CliValuePair pair => [pair],
            IEnumerable<CliValuePair> pairCollection => pairCollection,
            _ => null,
        };
    }

    private static void AddOptionValuePairs(
        List<string> args,
        OptionPart optionPart,
        IEnumerable<CliValuePair> pairs)
    {
        if (optionPart.Attribute.GetSeparator() != " ")
        {
            throw new InvalidOperationException(
                $"Two-operand CLI option property '{optionPart.PropertyName}' must use "
                + $"{nameof(OptionFormat)}.{nameof(OptionFormat.SpaceSeparated)}.");
        }

        var optionName = optionPart.Attribute.GetEffectiveName();
        foreach (var pair in pairs)
        {
            args.Add(optionName);
            args.Add(pair.First);
            args.Add(pair.Second);
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

    internal static string? GetSingleValue(object? rawValue)
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

        if (rawValue.GetType().IsEnum)
        {
            return ParseEnum(rawValue);
        }

        if (rawValue is Uri uri)
        {
            return ParseUri(uri);
        }

        return rawValue is IFormattable formattable
            ? formattable.ToString(null, CultureInfo.InvariantCulture)
            : rawValue.ToString()!;
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

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "Generated command metadata preserves the public enum fields inspected for EnumValueAttribute.")]
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
