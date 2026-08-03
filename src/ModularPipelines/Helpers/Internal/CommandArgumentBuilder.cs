using System.CodeDom.Compiler;
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
        var arguments = commandModel.OfType<ArgumentPart>().ToList();
        var flagsAndOptions = commandModel.Where(p => p is FlagPart or OptionPart).ToList();
        var renderedPhases = Enum.GetValues<CommandLinePhase>()
            .ToDictionary(
                phase => phase,
                phase => RenderPhase(
                    phase,
                    flagsAndOptions,
                    arguments,
                    optionsObject));

        if (renderedPhases[CommandLinePhase.EndOfOptions].Count > 0
            && renderedPhases[CommandLinePhase.Terminal].Count > 0)
        {
            throw new InvalidOperationException(
                "Terminal options cannot be combined with an end-of-options marker.");
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
        CommandLinePhase.EndOfOptions => 2,
        CommandLinePhase.Passthrough => 3,
        CommandLinePhase.Terminal => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null),
    };

    private static List<string> RenderPhase(
        CommandLinePhase phase,
        IEnumerable<PropertyCommandLinePart> flagsAndOptions,
        IEnumerable<ArgumentPart> arguments,
        object optionsObject)
    {
        var rendered = new List<string>();
        var phaseOptions = flagsAndOptions.Where(part => part.Phase == phase);
        var phaseArguments = arguments
            .Where(part => part.Phase == phase)
            .OrderBy(part => part.Attribute.Position);

        if (phase == CommandLinePhase.Terminal)
        {
            AddArguments(rendered, phaseArguments, optionsObject);
            AddFlagsAndOptions(rendered, phaseOptions, optionsObject);
        }
        else
        {
            AddFlagsAndOptions(rendered, phaseOptions, optionsObject);
            AddArguments(rendered, phaseArguments, optionsObject);
        }

        return rendered;
    }

    private static void AddArguments(
        List<string> args,
        IEnumerable<ArgumentPart>? argumentParts,
        object optionsObject)
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

            var rawValue = argumentPart.Getter(optionsObject);
            if (rawValue is null)
            {
                continue;
            }

            var values = GetValues(rawValue);
            if (argumentPart.Attribute.PrependOptionTerminator && values.Count > 0)
            {
                args.Add("--");
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
                    AddOption(args, optionPart, rawValue, optionsObject.GetType());
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

    private static void AddOption(
        List<string> args,
        OptionPart optionPart,
        object rawValue,
        Type optionsType)
    {
        if (optionPart.Attribute.ValueArity == CliOptionValueArity.Optional)
        {
            AddOptionalValueOption(args, optionPart, rawValue, optionsType);
            return;
        }

        var valuePairs = GetOptionValuePairs(rawValue);
        if (optionPart.Attribute.GroupValues)
        {
            var groupedValues = valuePairs is null
                ? GetValues(rawValue)
                : valuePairs.SelectMany(static pair => new[] { pair.First, pair.Second });
            AddGroupedOption(args, optionPart, groupedValues, optionsType);
            return;
        }

        if (valuePairs is not null)
        {
            AddOptionValuePairs(args, optionPart, valuePairs, optionsType);
            return;
        }

        var values = GetValues(rawValue);
        if (values.Count == 0)
        {
            throw CreateEmptyRequiredValueException(optionsType, optionPart);
        }

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw CreateEmptyRequiredValueException(optionsType, optionPart);
            }

            AddOptionValue(args, optionPart, value);
        }
    }

    private static void AddOptionalValueOption(
        List<string> args,
        OptionPart optionPart,
        object rawValue,
        Type optionsType)
    {
        var isLegacyGeneratedOption = IsLegacyGeneratedOption(optionsType, optionPart);
        var optionValues = rawValue switch
        {
            CliOptionValue optionValue => [optionValue],
            IEnumerable<CliOptionValue> values => values,
            // Preserve compatibility with generated option packages that predate CliOptionValue.
            string value when isLegacyGeneratedOption => [ToLegacyOptionalValue(value)],
            IEnumerable<string> values when isLegacyGeneratedOption => values.Select(ToLegacyOptionalValue),
            _ => throw CreateInvalidOptionalValueTypeException(optionsType, optionPart),
        };

        foreach (var optionValue in optionValues)
        {
            AddOptionalValue(args, optionPart, optionValue, optionsType);
        }
    }

    private static CliOptionValue ToLegacyOptionalValue(string value)
        => string.IsNullOrWhiteSpace(value) ? CliOptionValue.Bare : value;

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Legacy generated options retain their public CLI properties for reflection-based compatibility.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "Legacy generated option base types retain their public CLI properties for reflection-based compatibility.")]
    private static bool IsLegacyGeneratedOption(Type optionsType, OptionPart optionPart)
    {
        for (var currentType = optionsType; currentType is not null; currentType = currentType.BaseType)
        {
            var property = currentType.GetProperty(
                optionPart.PropertyName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            if (property is not null)
            {
                return currentType.GetCustomAttribute<GeneratedCodeAttribute>(inherit: false)?.Tool
                    == "ModularPipelines.OptionsGenerator";
            }
        }

        return false;
    }

    private static InvalidOperationException CreateInvalidOptionalValueTypeException(
        Type optionsType,
        OptionPart optionPart)
        => new(
            $"Optional-value CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
            + $"must use {nameof(CliOptionValue)} or IEnumerable<{nameof(CliOptionValue)}>.");

    private static void AddOptionalValue(
        List<string> args,
        OptionPart optionPart,
        CliOptionValue optionValue,
        Type optionsType)
    {
        if (optionValue.IsBare)
        {
            args.Add(optionPart.Attribute.GetEffectiveName());
            return;
        }

        if (string.IsNullOrWhiteSpace(optionValue.Value))
        {
            throw new InvalidOperationException(
                $"Optional-value CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
                + $"must use {nameof(CliOptionValue)}.{nameof(CliOptionValue.Bare)} or a non-empty value.");
        }

        AddOptionValue(args, optionPart, optionValue.Value);
    }

    private static void AddOptionValue(List<string> args, OptionPart optionPart, string value)
    {
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

    private static void AddGroupedOption(
        List<string> args,
        OptionPart optionPart,
        IEnumerable<string> values,
        Type optionsType)
    {
        if (optionPart.Attribute.GetSeparator() != " ")
        {
            throw new InvalidOperationException(
                $"Grouped option '{optionPart.Attribute.GetEffectiveName()}' must use a space separator.");
        }

        var renderedValues = values.ToList();
        if (renderedValues.Count == 0 || renderedValues.Any(string.IsNullOrWhiteSpace))
        {
            throw CreateEmptyRequiredValueException(optionsType, optionPart);
        }

        args.Add(optionPart.Attribute.GetEffectiveName());
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
        IEnumerable<CliValuePair> pairs,
        Type optionsType)
    {
        if (optionPart.Attribute.GetSeparator() != " ")
        {
            throw new InvalidOperationException(
                $"Two-operand CLI option property '{optionPart.PropertyName}' must use "
                + $"{nameof(OptionFormat)}.{nameof(OptionFormat.SpaceSeparated)}.");
        }

        var optionName = optionPart.Attribute.GetEffectiveName();
        var addedPair = false;
        foreach (var pair in pairs)
        {
            if (string.IsNullOrWhiteSpace(pair.First)
                || pair.Second is null)
            {
                throw CreateEmptyRequiredValueException(optionsType, optionPart);
            }

            args.Add(optionName);
            args.Add(pair.First);
            args.Add(pair.Second);
            addedPair = true;
        }

        if (!addedPair)
        {
            throw CreateEmptyRequiredValueException(optionsType, optionPart);
        }
    }

    private static InvalidOperationException CreateEmptyRequiredValueException(
        Type optionsType,
        OptionPart optionPart) =>
        new(
            $"Required CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
            + "cannot be empty or whitespace.");

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
