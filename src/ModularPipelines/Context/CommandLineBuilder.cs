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
/// 4. Insert phase-aware AdditionalArguments and combine command parts.
/// 5. Add manual Arguments if present.
/// 6. Render RunSettings as option-terminated pass-through arguments.
/// 7. Validate option terminators against terminal options in one place.
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
        var additionalArguments = options.AdditionalArguments?.ToList() ?? [];
        ValidateAdditionalArguments(additionalArguments);

        var terminalCommandModel = commandModel
            .Where(part => part.Phase == CommandLinePhase.Terminal)
            .ToList();
        var nonTerminalCommandModel = commandModel
            .Where(part => part.Phase != CommandLinePhase.Terminal)
            .ToList();
        var globalCommandModel = nonTerminalCommandModel.Where(part => part.IsGlobalOption).ToList();
        var commandSpecificModel = nonTerminalCommandModel.Where(part => !part.IsGlobalOption).ToList();
        var emittedOptionTerminator = false;
        var globalArgs = BuildNonTerminalArguments(
            globalCommandModel,
            additionalArguments,
            options,
            isGlobalOption: true,
            ref emittedOptionTerminator,
            out var globalOptionTerminatorIndex);
        var terminatorEmittedBeforeProperties = emittedOptionTerminator;
        var propertyArgs = BuildNonTerminalArguments(
            commandSpecificModel,
            additionalArguments,
            options,
            isGlobalOption: false,
            ref emittedOptionTerminator,
            out var commandOptionTerminatorIndex);
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
        var terminalAdditionalArgs = GetAdditionalArguments(
                additionalArguments,
                CommandLinePhase.Terminal)
            .ToList();
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
        if ((terminalAdditionalArgs.Count > 0 || terminalOptionArgs.Count > 0)
            && emittedOptionTerminator)
        {
            throw new InvalidOperationException(
                "Terminal options cannot be combined with arguments that emit or supply an "
                + "end-of-options marker. Remove either the terminal option or the '--' source.");
        }

        // Terminal options must follow every positional argument source.
        allArgs.AddRange(terminalArgumentArgs);
        allArgs.AddRange(terminalAdditionalArgs);
        allArgs.AddRange(terminalOptionArgs);

        return new CommandLine(tool, allArgs);
    }

    private List<string> BuildNonTerminalArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        IReadOnlyList<AdditionalCommandLineArgument> additionalArguments,
        CommandLineToolOptions options,
        bool isGlobalOption,
        ref bool emittedOptionTerminator,
        out int? emittedOptionTerminatorIndex)
    {
        var result = new List<string>();
        emittedOptionTerminatorIndex = null;

        foreach (var phase in Enum.GetValues<CommandLinePhase>()
                     .Where(static phase => phase != CommandLinePhase.Terminal))
        {
            var phaseAdditionalArguments = GetAdditionalArguments(
                    additionalArguments,
                    phase,
                    isGlobalOption)
                .ToList();
            if (phase == CommandLinePhaseCompatibility.LegacyEndOfOptions
                && phaseAdditionalArguments.Count > 0)
            {
                if (emittedOptionTerminator)
                {
                    throw new InvalidOperationException(
                        "An additional end-of-options marker cannot follow one that was already emitted.");
                }

                emittedOptionTerminatorIndex = result.Count;
                emittedOptionTerminator = true;
            }

            result.AddRange(phaseAdditionalArguments);

            var phaseModel = commandModel.Where(part => part.Phase == phase).ToList();
            var phaseArguments = _commandArgumentBuilder.BuildArguments(
                phaseModel,
                options,
                ref emittedOptionTerminator,
                out var phaseOptionTerminatorIndex);
            if (emittedOptionTerminatorIndex is null
                && phaseOptionTerminatorIndex is { } phaseIndex)
            {
                emittedOptionTerminatorIndex = result.Count + phaseIndex;
            }

            result.AddRange(phaseArguments);
        }

        return result;
    }

    private static IEnumerable<string> GetAdditionalArguments(
        IEnumerable<AdditionalCommandLineArgument> additionalArguments,
        CommandLinePhase phase,
        bool? isGlobalOption = null)
        => additionalArguments
            .Where(argument => argument.Phase == phase
                && (isGlobalOption is null || argument.IsGlobalOption == isGlobalOption))
            .Select(argument => argument.Value);

    private static void ValidateAdditionalArguments(
        IReadOnlyCollection<AdditionalCommandLineArgument> additionalArguments)
    {
        foreach (var argument in additionalArguments)
        {
            ArgumentNullException.ThrowIfNull(argument);

            if (!Enum.IsDefined(argument.Phase))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(CommandLineToolOptions.AdditionalArguments),
                    argument.Phase,
                    "The additional argument phase is not defined.");
            }

            ArgumentNullException.ThrowIfNull(argument.Value);

            if (argument is { IsGlobalOption: true, Phase: CommandLinePhase.Terminal })
            {
                throw new ArgumentException(
                    "A terminal additional argument cannot be a global option.",
                    nameof(CommandLineToolOptions.AdditionalArguments));
            }

            if (argument.Phase == CommandLinePhaseCompatibility.LegacyEndOfOptions
                && argument.Value != "--")
            {
                throw new ArgumentException(
                    "The legacy end-of-options phase only accepts the '--' marker.",
                    nameof(CommandLineToolOptions.AdditionalArguments));
            }

            if (argument.Value == "--"
                && argument.Phase != CommandLinePhaseCompatibility.LegacyEndOfOptions)
            {
                throw new ArgumentException(
                    "The '--' marker must use the legacy end-of-options phase.",
                    nameof(CommandLineToolOptions.AdditionalArguments));
            }
        }

        if (additionalArguments.Count(argument =>
                argument.Phase == CommandLinePhaseCompatibility.LegacyEndOfOptions) > 1)
        {
            throw new ArgumentException(
                "Additional arguments can contain at most one end-of-options marker.",
                nameof(CommandLineToolOptions.AdditionalArguments));
        }
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
        if (argument == "--" && options.ArgumentsContainOptionTerminator)
        {
            return null;
        }

        if (flagsByName.TryGetValue(argument, out var flag))
        {
            return new ManualOptionMatch(
                ArgumentCount: 1,
                flag.IsGlobalOption,
                IsTerminal: flag.Phase == CommandLinePhase.Terminal);
        }

        if (optionsByName.TryGetValue(argument, out var option))
        {
            return TryCreateManualOptionMatch(
                manualArgs,
                index,
                flagsByName,
                optionsByName,
                options,
                option,
                suppliedOperandCount: 0);
        }

        if (TryGetAttachedManualOption(argument, optionsByName, out var attachedOption))
        {
            return TryCreateManualOptionMatch(
                manualArgs,
                index,
                flagsByName,
                optionsByName,
                options,
                attachedOption,
                suppliedOperandCount: 1);
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

        return null;
    }

    private static ManualOptionMatch? TryCreateManualOptionMatch(
        IReadOnlyList<string> manualArgs,
        int index,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options,
        OptionPart option,
        int suppliedOperandCount)
    {
        var followingOperandCount = GetManualOperandCount(
            option,
            manualArgs,
            index,
            flagsByName,
            optionsByName,
            options,
            suppliedOperandCount);
        return followingOperandCount is { } count
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
        OptionPart? matchingOption = null;
        var matchingNameLength = 0;
        foreach (var item in optionsByName)
        {
            if (argument.Length <= item.Key.Length
                || item.Key.Length <= matchingNameLength
                || !argument.StartsWith(item.Key, StringComparison.Ordinal))
            {
                continue;
            }

            var separator = argument[item.Key.Length];
            if (item.Value.Attribute.Format == OptionFormat.NoSeparator
                || separator == '='
                || (separator == ':'
                    && item.Value.Attribute.Format == OptionFormat.ColonSeparated))
            {
                matchingOption = item.Value;
                matchingNameLength = item.Key.Length;
            }
        }

        option = matchingOption!;
        return matchingOption is not null;
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
                ValidateCombinedOptionScope(
                    argument,
                    index,
                    isGlobalOption,
                    flag.IsGlobalOption);
                isGlobalOption = flag.IsGlobalOption;
                isTerminal |= flag.Phase == CommandLinePhase.Terminal;
                continue;
            }

            if (!optionsByName.TryGetValue(shortName, out var option))
            {
                return false;
            }

            ValidateCombinedOptionScope(
                argument,
                index,
                isGlobalOption,
                option.IsGlobalOption);
            isGlobalOption = option.IsGlobalOption;
            isTerminal |= option.Phase == CommandLinePhase.Terminal;
            var hasAttachedOperand = index < argument.Length - 1;
            var operandCount = GetManualOperandCount(
                option,
                manualArgs,
                manualIndex,
                flagsByName,
                optionsByName,
                options,
                suppliedOperandCount: hasAttachedOperand ? 1 : 0);
            if (operandCount is null)
            {
                return false;
            }

            followingOperandCount = operandCount.Value;
            return true;
        }

        return true;
    }

    private static void ValidateCombinedOptionScope(
        string argument,
        int shortOptionIndex,
        bool previousIsGlobalOption,
        bool currentIsGlobalOption)
    {
        if (shortOptionIndex == 1 || previousIsGlobalOption == currentIsGlobalOption)
        {
            return;
        }

        throw new InvalidOperationException(
            $"Combined short-option cluster '{argument}' cannot mix global and "
            + "command-specific options. Split the cluster into separate arguments.");
    }

    private static int? GetManualOperandCount(
        OptionPart option,
        IReadOnlyList<string> manualArgs,
        int optionIndex,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options,
        int suppliedOperandCount)
    {
        var operandCount = GetConfiguredManualOperandCount(option, options);
        var remainingOperandCount = Math.Max(0, operandCount - suppliedOperandCount);
        if (option.Attribute.GroupValues)
        {
            var groupedOperandCount = 0;
            for (var index = optionIndex + 1; index < manualArgs.Count; index++)
            {
                if (IsRecognizedManualOptionToken(
                        manualArgs[index],
                        flagsByName,
                        optionsByName,
                        options.ArgumentsContainOptionTerminator))
                {
                    break;
                }

                groupedOperandCount++;
            }

            var minimumOperandCount = option.Attribute.ValueArity == CliOptionValueArity.Optional
                ? 0
                : remainingOperandCount;
            return groupedOperandCount >= minimumOperandCount
                ? groupedOperandCount
                : null;
        }

        if (option.Attribute.ValueArity != CliOptionValueArity.Optional
            || remainingOperandCount == 0)
        {
            return remainingOperandCount;
        }

        if (optionIndex + 1 >= manualArgs.Count)
        {
            return 0;
        }

        var possibleOperand = manualArgs[optionIndex + 1];
        return IsRecognizedManualOptionToken(
            possibleOperand,
            flagsByName,
            optionsByName,
            options.ArgumentsContainOptionTerminator)
            ? 0
            : remainingOperandCount;
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
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        bool argumentsContainOptionTerminator)
    {
        if ((argument == "--" && argumentsContainOptionTerminator)
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
