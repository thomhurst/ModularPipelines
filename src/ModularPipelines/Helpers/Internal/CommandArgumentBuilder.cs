using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
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

        var argumentsAfterOptions =
            argumentsByPlacement.GetValueOrDefault(ArgumentPlacement.AfterOptions) ?? [];

        var normal = RenderPhase(
            CommandLinePhase.Normal,
            flagsAndOptions,
            argumentsAfterOptions,
            optionsObject);
        var endOfOptions = RenderPhase(
            CommandLinePhase.EndOfOptions,
            flagsAndOptions,
            argumentsAfterOptions,
            optionsObject);
        var passthrough = RenderPhase(
            CommandLinePhase.Passthrough,
            flagsAndOptions,
            argumentsAfterOptions,
            optionsObject);
        var terminal = RenderPhase(
            CommandLinePhase.Terminal,
            flagsAndOptions,
            argumentsAfterOptions,
            optionsObject,
            argumentsFirst: true);

        if (endOfOptions.Count > 0 && terminal.Count > 0)
        {
            throw new InvalidOperationException(
                "Terminal options cannot be combined with an end-of-options marker.");
        }

        args.AddRange(normal);
        args.AddRange(endOfOptions);
        args.AddRange(passthrough);
        args.AddRange(terminal);

        return args;
    }

    private static List<string> RenderPhase(
        CommandLinePhase phase,
        IEnumerable<PropertyCommandLinePart> flagsAndOptions,
        IEnumerable<ArgumentPart> arguments,
        object optionsObject,
        bool argumentsFirst = false)
    {
        var rendered = new List<string>();
        var phaseOptions = flagsAndOptions.Where(part => part.Phase == phase);
        var phaseArguments = arguments.Where(part => part.Phase == phase);

        if (argumentsFirst)
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
        if (optionPart.ValueArity == CliOptionValueArity.None)
        {
            if (rawValue is not bool value || value)
            {
                args.Add(optionPart.Attribute.GetEffectiveName());
            }

            return;
        }

        if (optionPart.ValueArity == CliOptionValueArity.Optional)
        {
            AddOptionalValueOption(args, optionPart, rawValue, optionsType);
            return;
        }

        if (TryAddOptionValuePairs(args, optionPart, rawValue, optionsType))
        {
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
        var isLegacyGeneratedOptions = optionsType
            .GetCustomAttribute<GeneratedCodeAttribute>(inherit: false)
            ?.Tool == "ModularPipelines.OptionsGenerator";
        var optionValues = rawValue switch
        {
            CliOptionValue optionValue => [optionValue],
            IEnumerable<CliOptionValue> values when optionPart.Attribute.AllowMultiple => values,
            // Preserve compatibility with generated option packages that predate CliOptionValue.
            string value when isLegacyGeneratedOptions => [ToLegacyOptionalValue(value)],
            IEnumerable<string> values when optionPart.Attribute.AllowMultiple && isLegacyGeneratedOptions
                => values.Select(ToLegacyOptionalValue),
            _ => throw CreateInvalidOptionalValueTypeException(optionsType, optionPart),
        };

        foreach (var optionValue in optionValues)
        {
            AddOptionalValue(args, optionPart, optionValue, optionsType);
        }
    }

    private static CliOptionValue ToLegacyOptionalValue(string value)
        => string.IsNullOrWhiteSpace(value) ? CliOptionValue.Bare : value;

    private static InvalidOperationException CreateInvalidOptionalValueTypeException(
        Type optionsType,
        OptionPart optionPart)
        => new(
            $"Optional-value CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
            + $"must use {(optionPart.Attribute.AllowMultiple ? $"IEnumerable<{nameof(CliOptionValue)}>" : $"{nameof(CliOptionValue)}?")}.");

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

    private static bool TryAddOptionValuePairs(
        List<string> args,
        OptionPart optionPart,
        object rawValue,
        Type optionsType)
    {
        var pairs = rawValue switch
        {
            CliValuePair pair => [pair],
            IEnumerable<CliValuePair> pairCollection => pairCollection,
            _ => null,
        };

        if (pairs is null)
        {
            return false;
        }

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

        return true;
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
