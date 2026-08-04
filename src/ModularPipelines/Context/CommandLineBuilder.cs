using ModularPipelines.Attributes;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Builds a <see cref="CommandLine"/> from <see cref="CommandLineToolOptions"/>.
/// This is a pure transformation with no side effects.
/// </summary>
/// <remarks>
/// Uses existing internal helpers to:
/// 1. Resolve tool name from [CliTool] attribute or constructor parameter
/// 2. Get subcommand parts from [CliSubCommand] or a preferred [CliCommandAlias]
/// 3. Build arguments from [CliOption], [CliFlag], and [CliArgument] attributes
/// 4. Add manual Arguments if present
/// 5. Render RunSettings as option-terminated pass-through arguments.
/// 6. Validate option terminators against terminal options in one place.
/// </remarks>
internal sealed class CommandLineBuilder(
    IToolResolver toolResolver,
    ICommandPartsProvider commandPartsProvider,
    ICommandModelProvider commandModelProvider,
    ICommandArgumentBuilder commandArgumentBuilder) : ICommandLineBuilder
{
    private static readonly IReadOnlyList<PropertyCommandLinePart> RunSettingsCommandModel =
    [
        new ArgumentPart(
            nameof(CommandLineToolOptions.RunSettings),
            static options => ((CommandLineToolOptions) options).RunSettings,
            new CliArgumentAttribute
            {
                PrependOptionTerminator = true,
            }),
    ];

    private readonly IToolResolver _toolResolver = toolResolver;
    private readonly ICommandPartsProvider _commandPartsProvider = commandPartsProvider;
    private readonly ICommandModelProvider _commandModelProvider = commandModelProvider;
    private readonly ICommandArgumentBuilder _commandArgumentBuilder = commandArgumentBuilder;

    /// <inheritdoc />
    public CommandLine Build(CommandLineToolOptions options)
    {
        // 1. Resolve tool name using _toolResolver
        var tool = _toolResolver.ResolveTool(options)
            ?? throw new InvalidOperationException(
                $"Could not resolve tool name for {options.GetType().Name}. " +
                "Specify tool via [CliTool] attribute or constructor parameter.");

        // 2. Get static or runtime-computed command parts.
        var commandParts = _commandPartsProvider.GetRawCommandParts(options);

        // 3. Build arguments from properties using the command model. Properties declared
        // on a [CliGlobalOptions] base belong before the subcommand; command-specific
        // properties retain their normal position after it.
        var commandModel = _commandModelProvider.GetCommandModel(options.GetType());
        var terminalCommandModel = commandModel
            .Where(part => part.Phase == CommandLinePhase.Terminal)
            .ToList();
        var nonTerminalCommandModel = commandModel
            .Where(part => part.Phase != CommandLinePhase.Terminal)
            .ToList();
        var globalCommandModel = nonTerminalCommandModel.Where(part => part.IsGlobalOption).ToList();
        var commandSpecificModel = nonTerminalCommandModel.Where(part => !part.IsGlobalOption).ToList();
        var emittedOptionTerminator = false;
        var globalArgs = _commandArgumentBuilder.BuildArguments(
            globalCommandModel,
            options,
            ref emittedOptionTerminator,
            out var globalOptionTerminatorIndex).ToList();
        var terminatorEmittedBeforeProperties = emittedOptionTerminator;
        var propertyArgs = _commandArgumentBuilder.BuildArguments(
            commandSpecificModel,
            options,
            ref emittedOptionTerminator,
            out var commandOptionTerminatorIndex).ToList();
        var manualArgs = options.Arguments?.ToList() ?? [];
        ValidateManualOptionsAfterGlobalTerminator(
            options,
            manualArgs,
            commandSpecificModel,
            terminatorEmittedBeforeProperties);

        // Keep recognized manual options ahead of a marker emitted by a structured argument
        // or declared in the manual arguments or run settings; leave manual positional operands in place.
        var pendingTerminatorState = emittedOptionTerminator
                                     || options.ArgumentsContainOptionTerminator;
        var runSettingsArgs = _commandArgumentBuilder.BuildArguments(
            RunSettingsCommandModel,
            options,
            ref pendingTerminatorState);
        var terminalArgumentArgs = _commandArgumentBuilder.BuildArguments(
            [.. terminalCommandModel.Where(static part => part is ArgumentPart)],
            options,
            ref pendingTerminatorState);
        var hasOptionTerminator = pendingTerminatorState;
        var extractedManualOptions = options.ArgumentsContainToolOptions
                                     && hasOptionTerminator
            ? ExtractRecognizedManualOptionsByScope(
                manualArgs,
                globalCommandModel,
                [.. commandSpecificModel, .. terminalCommandModel],
                options,
                preserveTerminalOptions: true)
            : ExtractedManualOptions.Empty;
        ValidateTerminatorState(
            options,
            commandParts,
            manualArgs,
            extractedManualOptions,
            terminatorEmittedBeforeProperties,
            emittedOptionTerminator,
            hasOptionTerminator);
        var hasRenderedCommandOptions = ContainsRecognizedManualOption(
            propertyArgs,
            commandSpecificModel,
            options);
        InsertManualOptions(
            globalArgs,
            propertyArgs,
            extractedManualOptions,
            globalOptionTerminatorIndex,
            commandOptionTerminatorIndex,
            hasRenderedCommandOptions);

        emittedOptionTerminator = pendingTerminatorState;
        var terminalOptionArgs = _commandArgumentBuilder.BuildArguments(
            [.. terminalCommandModel.Where(static part => part is FlagPart or OptionPart)],
            options,
            ref emittedOptionTerminator);

        // 4. Combine: global args + command parts (subcommands) + property args
        // with any hoisted manual options before an emitted option terminator.
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(commandParts);
        allArgs.AddRange(propertyArgs);

        // 5. Add any manual arguments passed via options.Arguments
        allArgs.AddRange(manualArgs);

        // 6. Render RunSettings as option-terminated pass-through arguments.
        allArgs.AddRange(runSettingsArgs);

        // 7. A terminal option must not follow any rendered or manually supplied option terminator.
        if (terminalOptionArgs.Count > 0 && emittedOptionTerminator)
        {
            throw new InvalidOperationException(
                "Terminal options cannot be combined with arguments that emit or supply an "
                + "end-of-options marker. Remove either the terminal option or the '--' source.");
        }

        // Terminal options must follow every positional argument source.
        allArgs.AddRange(terminalArgumentArgs);
        allArgs.AddRange(terminalOptionArgs);

        return new CommandLine(tool, allArgs);
    }

    private static void ValidateTerminatorState(
        CommandLineToolOptions options,
        IReadOnlyCollection<string> commandParts,
        IReadOnlyCollection<string> manualArgs,
        ExtractedManualOptions extractedManualOptions,
        bool terminatorEmittedBeforeProperties,
        bool emittedOptionTerminator,
        bool hasOptionTerminator)
    {
        if (terminatorEmittedBeforeProperties && commandParts.Count > 0)
        {
            throw new InvalidOperationException(
                "A global end-of-options marker cannot precede a subcommand. "
                + "Remove the marker source or use options without a subcommand.");
        }

        if (options.ArgumentsContainOptionTerminator
            && !manualArgs.Contains("--", StringComparer.Ordinal))
        {
            throw new ArgumentException(
                $"{nameof(CommandLineToolOptions.ArgumentsContainOptionTerminator)} requires "
                + $"{nameof(CommandLineToolOptions.Arguments)} to contain an unconsumed '--'.",
                nameof(options));
        }

        if (options.ArgumentsContainToolOptions
            && hasOptionTerminator
            && extractedManualOptions.HasTerminalOptions)
        {
            throw new InvalidOperationException(
                "Manual terminal options cannot be combined with an end-of-options marker. "
                + "Remove either the terminal option or the '--' source.");
        }

        if (options.ArgumentsContainOptionTerminator && emittedOptionTerminator)
        {
            throw new InvalidOperationException(
                "Manual arguments cannot supply an end-of-options marker after one was already "
                + "emitted by a structured argument. Remove one of the '--' sources.");
        }
    }

    private static void InsertManualOptions(
        List<string> globalArgs,
        List<string> propertyArgs,
        ExtractedManualOptions extractedManualOptions,
        int? globalOptionTerminatorIndex,
        int? commandOptionTerminatorIndex,
        bool hasRenderedCommandOptions)
    {
        globalArgs.InsertRange(
            globalOptionTerminatorIndex ?? globalArgs.Count,
            extractedManualOptions.Global);
        if (commandOptionTerminatorIndex is { } insertionIndex)
        {
            propertyArgs.InsertRange(
                hasRenderedCommandOptions ? insertionIndex : 0,
                extractedManualOptions.Command);
        }
        else
        {
            propertyArgs.AddRange(extractedManualOptions.Command);
        }
    }

    private static void ValidateManualOptionsAfterGlobalTerminator(
        CommandLineToolOptions options,
        IReadOnlyCollection<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        bool terminatorEmittedBeforeProperties)
    {
        if (!options.ArgumentsContainToolOptions
            || !terminatorEmittedBeforeProperties
            || !ContainsRecognizedManualOption(manualArgs, commandModel, options))
        {
            return;
        }

        throw new InvalidOperationException(
            "Manual tool options cannot follow an end-of-options marker emitted by an "
            + "earlier property group. Remove either the manual option or the '--' source.");
    }

    private static IReadOnlyList<string> ExtractRecognizedManualOptions(
        List<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        CommandLineToolOptions options)
    {
        var extracted = ExtractRecognizedManualOptionsByScope(
            manualArgs,
            commandModel.Where(static part => part.IsGlobalOption).ToList(),
            commandModel.Where(static part => !part.IsGlobalOption).ToList(),
            options,
            preserveTerminalOptions: false);
        return [.. extracted.Global, .. extracted.Command];
    }

    private static ExtractedManualOptions ExtractRecognizedManualOptionsByScope(
        List<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> globalCommandModel,
        IReadOnlyList<PropertyCommandLinePart> commandSpecificModel,
        CommandLineToolOptions options,
        bool preserveTerminalOptions)
    {
        var commandModel = globalCommandModel.Concat(commandSpecificModel).ToList();
        var flagsByName = commandModel
            .OfType<FlagPart>()
            .SelectMany(static part => new[]
            {
                (Name: part.Attribute.Name, Part: part),
                (Name: part.Attribute.ShortForm, Part: part),
            })
            .Where(static item => item.Name is not null)
            .GroupBy(static item => item.Name!, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First().Part, StringComparer.Ordinal);
        var optionsByName = commandModel
            .OfType<OptionPart>()
            .SelectMany(static part => new[]
            {
                (Name: part.Attribute.Name, Part: part),
                (Name: part.Attribute.ShortForm, Part: part),
            })
            .Where(static item => item.Name is not null)
            .GroupBy(static item => item.Name!, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.First().Part, StringComparer.Ordinal);
        var globalOptions = new List<string>();
        var commandOptions = new List<string>();
        var remainingArguments = new List<string>();
        var hasTerminalOptions = false;
        for (var index = 0; index < manualArgs.Count;)
        {
            var match = TryMatchManualOption(
                manualArgs,
                index,
                flagsByName,
                optionsByName,
                options);
            if (match is null)
            {
                remainingArguments.Add(manualArgs[index]);
                index++;
                continue;
            }

            var matchedArguments = manualArgs.GetRange(index, match.Value.ArgumentCount);
            if (preserveTerminalOptions && match.Value.IsTerminal)
            {
                remainingArguments.AddRange(matchedArguments);
                hasTerminalOptions = true;
            }
            else
            {
                AddRecognizedManualOptions(
                    match.Value.IsGlobalOption,
                    matchedArguments,
                    globalOptions,
                    commandOptions);
            }
            index += match.Value.ArgumentCount;
        }

        if (globalOptions.Count == 0
            && commandOptions.Count == 0
            && !hasTerminalOptions)
        {
            return ExtractedManualOptions.Empty;
        }

        manualArgs.Clear();
        manualArgs.AddRange(remainingArguments);
        return new ExtractedManualOptions(
            globalOptions,
            commandOptions,
            hasTerminalOptions);
    }

    private static ManualOptionMatch? TryMatchManualOption(
        IReadOnlyList<string> manualArgs,
        int index,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options)
    {
        var argument = manualArgs[index];
        if (flagsByName.TryGetValue(argument, out var flag))
        {
            return new ManualOptionMatch(
                ArgumentCount: 1,
                flag.IsGlobalOption,
                IsTerminal: flag.Phase == CommandLinePhase.Terminal);
        }

        if (TryGetAttachedManualOption(argument, optionsByName, out var attachedOption))
        {
            return new ManualOptionMatch(
                ArgumentCount: 1,
                attachedOption.IsGlobalOption,
                IsTerminal: attachedOption.Phase == CommandLinePhase.Terminal);
        }

        if (TryGetCombinedShortOptionOperandCount(
                argument,
                manualArgs,
                index,
                flagsByName,
                optionsByName,
                options,
                out var combinedOperandCount,
                out var combinedIsGlobalOption,
                out var combinedIsTerminal))
        {
            return manualArgs.Count - index - 1 >= combinedOperandCount
                ? new ManualOptionMatch(
                    combinedOperandCount + 1,
                    combinedIsGlobalOption,
                    combinedIsTerminal)
                : null;
        }

        if (!optionsByName.TryGetValue(argument, out var option))
        {
            return null;
        }

        var operandCount = GetManualOperandCount(
            option,
            manualArgs,
            index,
            flagsByName,
            optionsByName,
            options);
        return operandCount is { } count
               && manualArgs.Count - index - 1 >= count
            ? new ManualOptionMatch(
                count + 1,
                option.IsGlobalOption,
                IsTerminal: option.Phase == CommandLinePhase.Terminal)
            : null;
    }

    private static void AddRecognizedManualOptions(
        bool isGlobalOption,
        IEnumerable<string> arguments,
        ICollection<string> globalOptions,
        ICollection<string> commandOptions)
    {
        var destination = isGlobalOption ? globalOptions : commandOptions;
        foreach (var argument in arguments)
        {
            destination.Add(argument);
        }
    }

    private static bool TryGetAttachedManualOption(
        string argument,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        out OptionPart option)
    {
        foreach (var item in optionsByName)
        {
            if (argument.Length <= item.Key.Length
                || !argument.StartsWith(item.Key, StringComparison.Ordinal))
            {
                continue;
            }

            var separator = argument[item.Key.Length];
            if (separator == '='
                || (separator == ':'
                    && item.Value.Attribute.Format == OptionFormat.ColonSeparated))
            {
                option = item.Value;
                return true;
            }
        }

        option = null!;
        return false;
    }

    private static bool ContainsRecognizedManualOption(
        IReadOnlyCollection<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        CommandLineToolOptions options) =>
        ExtractRecognizedManualOptions(manualArgs.ToList(), commandModel, options).Count > 0;

    private static bool TryGetCombinedShortOptionOperandCount(
        string argument,
        IReadOnlyList<string> manualArgs,
        int manualIndex,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options,
        out int followingOperandCount,
        out bool isGlobalOption,
        out bool isTerminal)
    {
        followingOperandCount = 0;
        isGlobalOption = true;
        isTerminal = false;
        if (argument.Length <= 2 || argument[0] != '-' || argument[1] == '-')
        {
            return false;
        }

        for (var index = 1; index < argument.Length; index++)
        {
            var shortName = $"-{argument[index]}";
            if (flagsByName.TryGetValue(shortName, out var flag))
            {
                isGlobalOption &= flag.IsGlobalOption;
                isTerminal |= flag.Phase == CommandLinePhase.Terminal;
                continue;
            }

            if (!optionsByName.TryGetValue(shortName, out var option))
            {
                return false;
            }

            isGlobalOption &= option.IsGlobalOption;
            isTerminal |= option.Phase == CommandLinePhase.Terminal;
            var operandCount = GetManualOperandCount(
                option,
                manualArgs,
                manualIndex,
                flagsByName,
                optionsByName,
                options);
            if (operandCount is null)
            {
                return false;
            }

            var hasAttachedOperand = index < argument.Length - 1;
            followingOperandCount = hasAttachedOperand
                ? Math.Max(0, operandCount.Value - 1)
                : operandCount.Value;
            return true;
        }

        return true;
    }

    private static int? GetManualOperandCount(
        OptionPart option,
        IReadOnlyList<string> manualArgs,
        int optionIndex,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options)
    {
        var operandCount = GetConfiguredManualOperandCount(option, options);
        if (option.Attribute.GroupValues)
        {
            var groupedOperandCount = 0;
            for (var index = optionIndex + 1; index < manualArgs.Count; index++)
            {
                if (IsRecognizedManualOptionToken(manualArgs[index], flagsByName, optionsByName))
                {
                    break;
                }

                groupedOperandCount++;
            }

            var minimumOperandCount = option.Attribute.ValueArity == CliOptionValueArity.Optional
                ? 0
                : operandCount;
            return groupedOperandCount >= minimumOperandCount
                ? groupedOperandCount
                : null;
        }

        if (option.Attribute.ValueArity != CliOptionValueArity.Optional
            || operandCount == 0
            || optionIndex + 1 >= manualArgs.Count)
        {
            return operandCount;
        }

        var possibleOperand = manualArgs[optionIndex + 1];
        return IsRecognizedManualOptionToken(possibleOperand, flagsByName, optionsByName)
            ? 0
            : operandCount;
    }

    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Reflection is used only for compatibility with legacy generated command metadata.")]
    private static int GetConfiguredManualOperandCount(
        OptionPart option,
        CommandLineToolOptions options)
    {
        if (option.ManualOperandCount >= 0)
        {
            return option.ManualOperandCount;
        }

        if (option.ManualOperandCount != -1)
        {
            throw new InvalidOperationException(
                $"Manual value count cannot be less than -1 for {option.PropertyName}.");
        }

        return CommandModelProvider.GetManualOperandCount(
            options.GetType(),
            option.PropertyName);
    }

    private static bool IsRecognizedManualOptionToken(
        string argument,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName)
    {
        if (argument == "--"
            || flagsByName.ContainsKey(argument)
            || optionsByName.ContainsKey(argument)
            || TryGetAttachedManualOption(argument, optionsByName, out _))
        {
            return true;
        }

        if (argument.Length <= 2 || argument[0] != '-' || argument[1] == '-')
        {
            return false;
        }

        for (var index = 1; index < argument.Length; index++)
        {
            var shortName = $"-{argument[index]}";
            if (!flagsByName.ContainsKey(shortName) && !optionsByName.ContainsKey(shortName))
            {
                return false;
            }

            if (optionsByName.ContainsKey(shortName))
            {
                return true;
            }
        }

        return true;
    }

    private readonly record struct ExtractedManualOptions(
        IReadOnlyList<string> Global,
        IReadOnlyList<string> Command,
        bool HasTerminalOptions)
    {
        public static ExtractedManualOptions Empty { get; } = new([], [], false);
    }

    private readonly record struct ManualOptionMatch(
        int ArgumentCount,
        bool IsGlobalOption,
        bool IsTerminal);
}
