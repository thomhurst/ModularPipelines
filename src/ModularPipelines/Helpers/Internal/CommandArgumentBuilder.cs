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
        var propertyValues = commandModel.ToDictionary(
            static part => part,
            part => part.Getter(optionsObject));
        var argumentValues = arguments.ToDictionary(
            static argument => argument,
            argument => (IReadOnlyList<string>) GetValues(propertyValues[argument]));
        var renderedOptionValues = flagsAndOptions.ToDictionary(
            static part => part,
            part => RenderOption(part, propertyValues[part]));
        ValidateOptionTerminatorOrdering(arguments, renderedOptionValues, argumentValues);
        var renderedPhases = new Dictionary<CommandLinePhase, IReadOnlyList<string>>();
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

        return
        [
            .. renderedPhases
                .OrderBy(pair => GetRenderOrder(pair.Key))
                .SelectMany(pair => pair.Value),
        ];
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
        IReadOnlyDictionary<PropertyCommandLinePart, IReadOnlyList<string>> renderedOptionValues,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues,
        Type optionsType,
        ref bool emittedOptionTerminator)
    {
        var rendered = new List<string>();
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
                ref emittedOptionTerminator);
            AddFlagsAndOptions(rendered, phaseOptions, renderedOptionValues);
        }
        else
        {
            AddFlagsAndOptions(rendered, phaseOptions, renderedOptionValues);
            AddArguments(
                rendered,
                phaseArguments,
                argumentValues,
                optionsType,
                ref emittedOptionTerminator);
        }

        return rendered;
    }

    private static void AddArguments(
        List<string> args,
        IEnumerable<ArgumentPart>? argumentParts,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues,
        Type optionsType,
        ref bool emittedOptionTerminator)
    {
        if (argumentParts is null)
        {
            return;
        }

        foreach (var argumentPart in argumentParts)
        {
            var values = argumentValues[argumentPart];
            if (argumentPart.Attribute.Required && IsEmpty(values))
            {
                throw new ArgumentException(
                    $"Required CLI argument '{optionsType.Name}.{argumentPart.PropertyName}' cannot be null or empty.",
                    argumentPart.PropertyName);
            }

            if (values.Count == 0)
            {
                continue;
            }

            if (RequiresOptionTerminator(argumentPart, values)
                && !emittedOptionTerminator)
            {
                args.Add("--");
                emittedOptionTerminator = true;
            }

            args.AddRange(values);
        }
    }

    private static bool IsEmpty(IReadOnlyCollection<string> values) =>
        values.Count == 0 || values.All(string.IsNullOrWhiteSpace);

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
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> argumentValues)
    {
        var renderedOptions = renderedOptionValues
            .Where(static pair => pair.Value.Count > 0)
            .Select(static pair => pair.Key)
            .ToArray();
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
        object? rawValue)
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
                AddOption(rendered, option, rawValue);
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
                    args.Add(GetEffectiveName(optionPart.Attribute));
                }

                continue;
            }

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
    }

    private static void AddGroupedOption(
        List<string> args,
        CliOptionAttribute attribute,
        IEnumerable<string> values)
    {
        if (GetSeparator(attribute) != " ")
        {
            throw new InvalidOperationException(
                $"Grouped option '{GetEffectiveName(attribute)}' must use a space separator.");
        }

        var renderedValues = values.Where(value => !string.IsNullOrWhiteSpace(value)).ToList();
        if (renderedValues.Count == 0)
        {
            if (attribute.ValueArity == CliOptionValueArity.Optional)
            {
                args.Add(GetEffectiveName(attribute));
            }

            return;
        }

        args.Add(GetEffectiveName(attribute));
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
        if (GetSeparator(optionPart.Attribute) != " ")
        {
            throw new InvalidOperationException(
                $"Two-operand CLI option property '{optionPart.PropertyName}' must use "
                + $"{nameof(OptionFormat)}.{nameof(OptionFormat.SpaceSeparated)}.");
        }

        var optionName = GetEffectiveName(optionPart.Attribute);
        foreach (var pair in pairs)
        {
            args.Add(optionName);
            args.Add(pair.First);
            args.Add(pair.Second);
        }
    }

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
