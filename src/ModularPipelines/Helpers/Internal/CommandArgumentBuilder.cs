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
        var emittedOptionTerminator = false;
        return BuildArguments(commandModel, optionsObject, ref emittedOptionTerminator);
    }

    /// <inheritdoc/>
    public IReadOnlyList<string> BuildArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        object optionsObject,
        ref bool emittedOptionTerminator) =>
        BuildArguments(commandModel, optionsObject, ref emittedOptionTerminator, out _);

    /// <inheritdoc/>
    public IReadOnlyList<string> BuildArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        object optionsObject,
        ref bool emittedOptionTerminator,
        out int? emittedOptionTerminatorIndex)
    {
        var arguments = commandModel.OfType<ArgumentPart>().ToList();
        var flagsAndOptions = commandModel.Where(p => p is FlagPart or OptionPart).ToList();
        var propertyValues = commandModel.ToDictionary(
            static part => part,
            part => part.Getter(optionsObject));
        var argumentValues = arguments.ToDictionary(
            static argument => argument,
            argument => (IReadOnlyList<string>) GetValues(propertyValues[argument]));
        var renderedOptionValues = flagsAndOptions.ToDictionary(
            static part => part,
            part => RenderOption(part, propertyValues[part], optionsObject.GetType()));
        ValidateOptionTerminatorOrdering(
            arguments,
            renderedOptionValues,
            argumentValues,
            emittedOptionTerminator);
        var renderedPhases = new Dictionary<CommandLinePhase, RenderedPhase>();
        foreach (var phase in Enum.GetValues<CommandLinePhase>())
        {
            renderedPhases.Add(
                phase,
                RenderPhase(
                    phase,
                    flagsAndOptions,
                    arguments,
                    renderedOptionValues,
                    argumentValues,
                    optionsObject.GetType(),
                    ref emittedOptionTerminator));
        }

        var rendered = new List<string>();
        emittedOptionTerminatorIndex = null;
        foreach (var phase in renderedPhases.OrderBy(pair => GetRenderOrder(pair.Key)))
        {
            if (emittedOptionTerminatorIndex is null
                && phase.Value.OptionTerminatorIndex is { } phaseIndex)
            {
                emittedOptionTerminatorIndex = rendered.Count + phaseIndex;
            }

            rendered.AddRange(phase.Value.Arguments);
        }

        return rendered;
    }

    private static int GetRenderOrder(CommandLinePhase phase) => phase switch
    {
        CommandLinePhase.EarlyOperand => 0,
        CommandLinePhase.Normal => 1,
        CommandLinePhaseCompatibility.LegacyEndOfOptions => 2,
        CommandLinePhase.Passthrough => 3,
        CommandLinePhase.Terminal => 4,
        _ => throw new ArgumentOutOfRangeException(nameof(phase), phase, null),
    };

    private static RenderedPhase RenderPhase(
        CommandLinePhase phase,
        IEnumerable<PropertyCommandLinePart> flagsAndOptions,
        IEnumerable<ArgumentPart> arguments,
        IReadOnlyDictionary<PropertyCommandLinePart, IReadOnlyList<string>> renderedOptionValues,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues,
        Type optionsType,
        ref bool emittedOptionTerminator)
    {
        var rendered = new List<string>();
        int? optionTerminatorIndex = null;
        var phaseOptions = flagsAndOptions.Where(part => part.Phase == phase);
        var phaseArguments = arguments
            .Where(part => part.Phase == phase)
            .OrderBy(part => part.Attribute.Position);

        if (phase == CommandLinePhase.Terminal)
        {
            AddArguments(
                rendered,
                phaseArguments,
                argumentValues,
                optionsType,
                ref emittedOptionTerminator,
                ref optionTerminatorIndex);
            AddFlagsAndOptions(rendered, phaseOptions, renderedOptionValues);
        }
        else
        {
            AddFlagsAndOptions(rendered, phaseOptions, renderedOptionValues);
            if (phase == CommandLinePhaseCompatibility.LegacyEndOfOptions
                && rendered.IndexOf("--") is var terminatorIndex
                && terminatorIndex >= 0)
            {
                optionTerminatorIndex = terminatorIndex;
                emittedOptionTerminator = true;
            }

            AddArguments(
                rendered,
                phaseArguments,
                argumentValues,
                optionsType,
                ref emittedOptionTerminator,
                ref optionTerminatorIndex);
        }

        return new RenderedPhase(rendered, optionTerminatorIndex);
    }

    private static void AddArguments(
        List<string> args,
        IEnumerable<ArgumentPart>? argumentParts,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues,
        Type optionsType,
        ref bool emittedOptionTerminator,
        ref int? optionTerminatorIndex)
    {
        if (argumentParts is null)
        {
            return;
        }

        foreach (var argumentPart in argumentParts)
        {
            var values = argumentValues[argumentPart];
            if (argumentPart.Attribute.Required && values.Count == 0)
            {
                throw new ArgumentException(
                    $"Required CLI argument '{optionsType.Name}.{argumentPart.PropertyName}' cannot be null.",
                    argumentPart.PropertyName);
            }

            if (values.Count == 0)
            {
                continue;
            }

            if (RequiresOptionTerminator(argumentPart, values)
                && !emittedOptionTerminator)
            {
                optionTerminatorIndex = args.Count;
                args.Add("--");
                emittedOptionTerminator = true;
            }

            args.AddRange(values);
        }
    }

    private sealed record RenderedPhase(
        IReadOnlyList<string> Arguments,
        int? OptionTerminatorIndex);

    private static void AddFlagsAndOptions(
        List<string> args,
        IEnumerable<PropertyCommandLinePart> parts,
        IReadOnlyDictionary<PropertyCommandLinePart, IReadOnlyList<string>> renderedOptionValues)
    {
        foreach (var part in parts)
        {
            args.AddRange(renderedOptionValues[part]);
        }
    }

    private static void ValidateOptionTerminatorOrdering(
        IEnumerable<ArgumentPart> arguments,
        IReadOnlyDictionary<PropertyCommandLinePart, IReadOnlyList<string>> renderedOptionValues,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues,
        bool optionTerminatorAlreadyEmitted)
    {
        var renderedOptions = renderedOptionValues
            .Where(static pair => pair.Value.Count > 0)
            .Select(static pair => pair.Key)
            .ToArray();
        if (optionTerminatorAlreadyEmitted && renderedOptions.Length > 0)
        {
            throw new InvalidOperationException(
                "CLI flags or options cannot be rendered after an end-of-options marker "
                + "emitted by an earlier property group.");
        }

        var legacyOptionTerminatorRendered = renderedOptionValues.Any(static pair =>
            pair.Key.Phase == CommandLinePhaseCompatibility.LegacyEndOfOptions
            && pair.Value.Contains("--", StringComparer.Ordinal));
        if (legacyOptionTerminatorRendered
            && renderedOptions.Any(static option =>
                GetRenderOrder(option.Phase) > GetRenderOrder(CommandLinePhaseCompatibility.LegacyEndOfOptions)))
        {
            throw new InvalidOperationException(
                "CLI flags or options cannot be rendered after a legacy end-of-options marker.");
        }

        foreach (var argument in arguments)
        {
            var values = argumentValues[argument];
            if (values.Count == 0 || !RequiresOptionTerminator(argument, values))
            {
                continue;
            }

            if (renderedOptions.Any(option => IsRenderedAfter(option, argument)))
            {
                throw new InvalidOperationException(
                    $"CLI argument '{argument.PropertyName}' emits an end-of-options marker before " +
                    "a later flag or option. Move the argument to a later phase or remove its " +
                    "option-terminator setting.");
            }
        }
    }

    private static IReadOnlyList<string> RenderOption(
        PropertyCommandLinePart part,
        object? rawValue,
        Type optionsType)
    {
        if (rawValue is null)
        {
            return [];
        }

        var rendered = new List<string>();
        switch (part)
        {
            case FlagPart flag:
                AddFlag(rendered, flag, rawValue);
                break;
            case OptionPart option:
                AddOption(rendered, option, rawValue, optionsType);
                break;
        }

        return rendered;
    }

    private static bool IsRenderedAfter(
        PropertyCommandLinePart option,
        ArgumentPart argument)
    {
        var optionOrder = GetRenderOrder(option.Phase);
        var argumentOrder = GetRenderOrder(argument.Phase);
        return optionOrder > argumentOrder
               || (optionOrder == argumentOrder
                   && argument.Phase == CommandLinePhase.Terminal);
    }

    private static bool RequiresOptionTerminator(
        ArgumentPart argument,
        IReadOnlyCollection<string> values) =>
        argument.Attribute.PrependOptionTerminator
        || (argument.Attribute.PrependOptionTerminatorIfValueStartsWithDash
            && values.Any(static value => value.StartsWith('-')));

    private static void AddFlag(List<string> args, FlagPart flagPart, object rawValue)
    {
        if (rawValue is bool boolValue && boolValue)
        {
            args.Add(GetEffectiveName(flagPart.Attribute));
        }

        if (rawValue is int count && count > 0)
        {
            args.AddRange(Enumerable.Repeat(GetEffectiveName(flagPart.Attribute), count));
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
            if (optionPart.Attribute.GroupValues)
            {
                AddGroupedOptionalValueOption(args, optionPart, rawValue, optionsType);
            }
            else
            {
                AddOptionalValueOption(args, optionPart, rawValue, optionsType);
            }

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
            return;
        }

        foreach (var value in values)
        {
            AddOptionValue(args, optionPart, value);
        }
    }

    private static void AddOptionalValueOption(
        List<string> args,
        OptionPart optionPart,
        object rawValue,
        Type optionsType)
    {
        foreach (var optionValue in GetOptionalValues(rawValue, optionsType, optionPart))
        {
            AddOptionalValue(args, optionPart, optionValue, optionsType);
        }
    }

    private static void AddGroupedOptionalValueOption(
        List<string> args,
        OptionPart optionPart,
        object rawValue,
        Type optionsType)
    {
        var optionValues = GetOptionalValues(rawValue, optionsType, optionPart).ToList();
        if (optionValues.Count == 0)
        {
            return;
        }

        if (GetSeparator(optionPart.Attribute) != " ")
        {
            throw new InvalidOperationException(
                $"Grouped option '{GetEffectiveName(optionPart.Attribute)}' must use a space separator.");
        }

        foreach (var optionValue in optionValues.Where(static value => !value.IsBare))
        {
            ValidateOptionalValue(optionValue, optionsType, optionPart);
        }

        args.Add(GetEffectiveName(optionPart.Attribute));
        args.AddRange(optionValues
            .Where(static value => !value.IsBare)
            .Select(static value => value.Value!));
    }

    private static IEnumerable<CliOptionValue> GetOptionalValues(
        object rawValue,
        Type optionsType,
        OptionPart optionPart)
    {
        var isLegacyGeneratedOption = IsLegacyGeneratedOption(optionsType, optionPart);
        return rawValue switch
        {
            CliOptionValue optionValue => [optionValue],
            IEnumerable<CliOptionValue> values => values.OfType<CliOptionValue>(),
            // Preserve compatibility with generated option packages that predate CliOptionValue.
            string value when isLegacyGeneratedOption => [ToLegacyOptionalValue(value)],
            IEnumerable<string> values when isLegacyGeneratedOption => values
                .OfType<string>()
                .Select(ToLegacyOptionalValue),
            _ => throw CreateInvalidOptionalValueTypeException(optionsType, optionPart),
        };
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
            args.Add(GetEffectiveName(optionPart.Attribute));
            return;
        }

        ValidateOptionalValue(optionValue, optionsType, optionPart);
        AddOptionValue(args, optionPart, optionValue.Value!);
    }

    private static void ValidateOptionalValue(
        CliOptionValue optionValue,
        Type optionsType,
        OptionPart optionPart)
    {
        if (optionValue.Value is null)
        {
            throw new InvalidOperationException(
                $"Optional-value CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
                + $"must use {nameof(CliOptionValue)}.{nameof(CliOptionValue.Bare)} or an explicit value.");
        }
    }

    private static void AddOptionValue(List<string> args, OptionPart optionPart, string value)
    {
        var optionName = GetEffectiveName(optionPart.Attribute);
        var separator = GetSeparator(optionPart.Attribute);

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
        if (GetSeparator(optionPart.Attribute) != " ")
        {
            throw new InvalidOperationException(
                $"Grouped option '{GetEffectiveName(optionPart.Attribute)}' must use a space separator.");
        }

        var renderedValues = values.ToList();
        if (renderedValues.Count == 0)
        {
            return;
        }

        if (renderedValues.Any(static value => value is null))
        {
            throw CreateNullRequiredValueException(optionsType, optionPart);
        }

        args.Add(GetEffectiveName(optionPart.Attribute));
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
        if (GetSeparator(optionPart.Attribute) != " ")
        {
            throw new InvalidOperationException(
                $"Two-operand CLI option property '{optionPart.PropertyName}' must use "
                + $"{nameof(OptionFormat)}.{nameof(OptionFormat.SpaceSeparated)}.");
        }

        var optionName = GetEffectiveName(optionPart.Attribute);
        foreach (var pair in pairs)
        {
            if (pair.First is null || pair.Second is null)
            {
                throw CreateNullRequiredValueException(optionsType, optionPart);
            }

            args.Add(optionName);
            args.Add(pair.First);
            args.Add(pair.Second);
        }
    }

    private static InvalidOperationException CreateNullRequiredValueException(
        Type optionsType,
        OptionPart optionPart) =>
        new(
            $"Required CLI option property '{optionsType.FullName}.{optionPart.PropertyName}' "
            + "cannot contain null values.");

    private static string GetEffectiveName(CliFlagAttribute attribute) =>
        attribute.PreferShortForm && !string.IsNullOrEmpty(attribute.ShortForm)
            ? attribute.ShortForm
            : attribute.Name;

    private static string GetEffectiveName(CliOptionAttribute attribute) =>
        attribute.PreferShortForm && !string.IsNullOrEmpty(attribute.ShortForm)
            ? attribute.ShortForm
            : attribute.Name;

    private static string GetSeparator(CliOptionAttribute attribute)
    {
        return attribute.Format switch
        {
            OptionFormat.SpaceSeparated => " ",
            OptionFormat.EqualsSeparated => "=",
            OptionFormat.ColonSeparated => ":",
            OptionFormat.NoSeparator => string.Empty,
            _ => " ",
        };
    }

    private static List<string> GetValues(object? rawValue)
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
