using ModularPipelines.Attributes;
using ModularPipelines.Engine;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Context;

/// <summary>
/// Builds a <see cref="CommandLine"/> from <see cref="CommandLineToolOptions"/>.
/// This is a pure transformation with no side effects.
/// </summary>
/// <remarks>
/// Uses existing internal helpers to:
/// 1. Validate DataAnnotations on the options object.
/// 2. Resolve tool name from [CliTool] attribute or constructor parameter.
/// 3. Get subcommand parts from [CliSubCommand] or a preferred [CliCommandAlias].
/// 4. Build arguments from [CliOption], [CliFlag], and [CliArgument] attributes.
/// 5. Insert phase-aware AdditionalArguments and combine command parts.
/// 6. Add manual Arguments if present.
/// 7. Render RunSettings as option-terminated pass-through arguments.
/// 8. Validate option terminators against terminal options in one place.
/// </remarks>
internal sealed class CommandLineBuilder(
    IToolResolver toolResolver,
    ICommandPartsProvider commandPartsProvider,
    ICommandModelProvider commandModelProvider,
    ICommandArgumentBuilder commandArgumentBuilder,
    IServiceProvider serviceProvider,
    ISecretObfuscator secretObfuscator) : ICommandLineBuilder
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
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ISecretObfuscator _secretObfuscator = secretObfuscator;

    /// <inheritdoc />
    public CommandLine Build(CommandLineToolOptions options)
    {
        CommandLineOptionsValidator.Validate(options, _serviceProvider, _secretObfuscator);

        // 2. Resolve tool name using _toolResolver
        var tool = _toolResolver.ResolveTool(options)
            ?? throw new InvalidOperationException(
                $"Could not resolve tool name for {options.GetType().Name}. " +
                "Specify tool via [CliTool] attribute or constructor parameter.");

        // 3. Get static or runtime-computed command parts.
        var commandParts = _commandPartsProvider.GetRawCommandParts(options);

        // 4. Build arguments from properties using the command model. Properties declared
        // on a [CliGlobalOptions] base belong before the subcommand; command-specific
        // properties retain their normal position after it.
        var commandModel = _commandModelProvider.GetCommandModel(options.GetType());
        var additionalArguments = options.AdditionalArguments?.ToList() ?? [];
        var manualArgs = options.Arguments?.ToList() ?? [];
        var requiredOperandMatch = MatchManualRequiredOperands(
            commandModel,
            options,
            manualArgs);
        var manualOptionTerminatorRemains = options.ArgumentsContainOptionTerminator
                                            && !requiredOperandMatch.ConsumedOptionTerminator;
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
            manualRequiredOperands: null,
            requiredOperandMatch.MaterializedValues,
            ref emittedOptionTerminator,
            out var globalOptionTerminatorIndex);
        var terminatorEmittedBeforeProperties = emittedOptionTerminator;
        var propertyArgs = BuildNonTerminalArguments(
            commandSpecificModel,
            additionalArguments,
            options,
            isGlobalOption: false,
            requiredOperandMatch.ManualValues,
            requiredOperandMatch.MaterializedValues,
            ref emittedOptionTerminator,
            out var commandOptionTerminatorIndex);
        ValidateManualOptionsAfterGlobalTerminator(
            options,
            manualArgs,
            commandSpecificModel,
            terminatorEmittedBeforeProperties);

        var modelEmittedOptionTerminator = emittedOptionTerminator;
        var terminalArgumentTerminatorState = emittedOptionTerminator
                                              || manualOptionTerminatorRemains;
        var terminalArgumentArgs = _commandArgumentBuilder.BuildArguments(
            [.. terminalCommandModel.Where(static part => part is ArgumentPart)],
            options,
            ref terminalArgumentTerminatorState,
            out var terminalArgumentOptionTerminatorIndex,
            requiredOperandMatch.ManualValues,
            requiredOperandMatch.MaterializedValues);
        modelEmittedOptionTerminator |= terminalArgumentOptionTerminatorIndex is not null;

        // Keep recognized manual options ahead of a marker emitted by a structured argument
        // or declared in the manual arguments or run settings; leave manual positional operands in place.
        var pendingTerminatorState = modelEmittedOptionTerminator
                                     || manualOptionTerminatorRemains;
        var runSettingsArgs = _commandArgumentBuilder.BuildArguments(
            RunSettingsCommandModel,
            options,
            ref pendingTerminatorState);
        ValidateRunSettingsTerminator(modelEmittedOptionTerminator, runSettingsArgs);
        var terminalAdditionalArgs = GetAdditionalArguments(
                additionalArguments,
                CommandLinePhase.Terminal)
            .ToList();
        var hasOptionTerminator = pendingTerminatorState;
        var extractedManualOptions = ExtractManualOptionsBeforeTerminator(
            options,
            hasOptionTerminator,
            manualArgs,
            globalCommandModel,
            commandSpecificModel,
            terminalCommandModel);
        ValidateTerminatorState(
            options,
            commandParts,
            manualArgs,
            extractedManualOptions,
            manualOptionTerminatorRemains,
            terminatorEmittedBeforeProperties,
            modelEmittedOptionTerminator,
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

        // 5. Combine: global args + command parts (subcommands) + property args
        // with any hoisted manual options before an emitted option terminator.
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(commandParts);
        allArgs.AddRange(propertyArgs);

        // 6. Add any manual arguments passed via options.Arguments
        allArgs.AddRange(manualArgs);

        // 7. Render RunSettings as option-terminated pass-through arguments.
        allArgs.AddRange(runSettingsArgs);

        // 8. A terminal option must not follow any rendered or manually supplied option terminator.
        ValidateTerminalOptions(
            terminalAdditionalArgs,
            terminalOptionArgs,
            emittedOptionTerminator);

        // Terminal options must follow every positional argument source.
        allArgs.AddRange(terminalArgumentArgs);
        allArgs.AddRange(terminalAdditionalArgs);
        allArgs.AddRange(terminalOptionArgs);

        return new CommandLine(tool, allArgs);
    }

    private static ExtractedManualOptions ExtractManualOptionsBeforeTerminator(
        CommandLineToolOptions options,
        bool hasOptionTerminator,
        List<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> globalCommandModel,
        IReadOnlyList<PropertyCommandLinePart> commandSpecificModel,
        IReadOnlyList<PropertyCommandLinePart> terminalCommandModel)
    {
        if (!options.ArgumentsContainToolOptions || !hasOptionTerminator)
        {
            return ExtractedManualOptions.Empty;
        }

        return ExtractRecognizedManualOptionsByScope(
            manualArgs,
            globalCommandModel,
            [.. commandSpecificModel, .. terminalCommandModel],
            options,
            preserveTerminalOptions: true);
    }

    private static void ValidateTerminalOptions(
        IReadOnlyCollection<string> terminalAdditionalArgs,
        IReadOnlyCollection<string> terminalOptionArgs,
        bool emittedOptionTerminator)
    {
        if (!emittedOptionTerminator
            || (terminalAdditionalArgs.Count == 0 && terminalOptionArgs.Count == 0))
        {
            return;
        }

        throw new InvalidOperationException(
            "Terminal options cannot be combined with arguments that emit or supply an "
            + "end-of-options marker. Remove either the terminal option or the '--' source.");
    }

    private static void ValidateRunSettingsTerminator(
        bool modelEmittedOptionTerminator,
        IReadOnlyCollection<string> runSettingsArgs)
    {
        if (modelEmittedOptionTerminator && runSettingsArgs.Count > 0)
        {
            throw new InvalidOperationException(
                $"{nameof(CommandLineToolOptions.RunSettings)} cannot be combined with a structured "
                + "argument that emits an end-of-options marker. Remove one of the '--' sources.");
        }
    }

    private List<string> BuildNonTerminalArguments(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        IReadOnlyList<AdditionalCommandLineArgument> additionalArguments,
        CommandLineToolOptions options,
        bool isGlobalOption,
        IReadOnlyDictionary<ArgumentPart, string>? manualRequiredOperands,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> materializedRequiredOperands,
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
            result.AddRange(phaseAdditionalArguments);

            var phaseModel = commandModel.Where(part => part.Phase == phase).ToList();
            var phaseArguments = _commandArgumentBuilder.BuildArguments(
                phaseModel,
                options,
                ref emittedOptionTerminator,
                out var phaseOptionTerminatorIndex,
                manualRequiredOperands,
                materializedRequiredOperands);
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

            if (argument.Value == "--")
            {
                throw new ArgumentException(
                    "The '--' marker must be emitted by CliArgumentAttribute option-terminator settings "
                    + "or declared manual arguments.",
                    nameof(CommandLineToolOptions.AdditionalArguments));
            }
        }
    }

    private static void ValidateTerminatorState(
        CommandLineToolOptions options,
        IReadOnlyCollection<string> commandParts,
        IReadOnlyCollection<string> manualArgs,
        ExtractedManualOptions extractedManualOptions,
        bool manualOptionTerminatorRemains,
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

        if (manualOptionTerminatorRemains
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

        if (manualOptionTerminatorRemains && emittedOptionTerminator)
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

    private static RequiredOperandMatch MatchManualRequiredOperands(
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        CommandLineToolOptions options,
        List<string> manualArgs)
    {
        var requiredOperands = commandModel
            .OfType<ArgumentPart>()
            .Where(part => !part.IsGlobalOption
                           && part.Attribute.Required)
            .OrderBy(static part => CommandLinePhaseOrder.GetRenderOrder(part.Phase))
            .ThenBy(static part => part.Attribute.Position)
            .ToList();
        var materializedValues = requiredOperands.ToDictionary(
            static part => part,
            part => (IReadOnlyList<string>) CommandArgumentBuilder.GetValues(part.Getter(options)));
        var missingRequiredOperands = requiredOperands
            .Where(part => materializedValues[part].Count == 0)
            .ToList();
        if (missingRequiredOperands.Count == 0 || manualArgs.Count == 0)
        {
            return new RequiredOperandMatch(
                new Dictionary<ArgumentPart, string>(),
                materializedValues,
                false);
        }

        IReadOnlyList<int> operandIndices;
        if (options.ArgumentsContainToolOptions)
        {
            var classifiedArguments = manualArgs.ToList();
            var classifiedOperandIndices = new List<int>();
            _ = ExtractRecognizedManualOptionsByScope(
                classifiedArguments,
                commandModel.Where(static part => part.IsGlobalOption).ToList(),
                commandModel.Where(static part => !part.IsGlobalOption).ToList(),
                options,
                preserveTerminalOptions: false,
                positionalArgumentIndices: classifiedOperandIndices);
            operandIndices = classifiedOperandIndices;
        }
        else
        {
            operandIndices = Enumerable.Range(0, manualArgs.Count)
                .Where(index => manualArgs[index] != "--")
                .ToList();
        }

        var matchedOperandCount = Math.Min(missingRequiredOperands.Count, operandIndices.Count);
        var result = new Dictionary<ArgumentPart, string>(matchedOperandCount);
        var indicesToRemove = new HashSet<int>();
        var consumedOptionTerminator = false;
        for (var index = 0; index < matchedOperandCount; index++)
        {
            var operandIndex = operandIndices[index];
            var requiredOperand = missingRequiredOperands[index];
            result.Add(requiredOperand, manualArgs[operandIndex]);
            indicesToRemove.Add(operandIndex);
            if (requiredOperand.Attribute.PrependOptionTerminator
                && operandIndex > 0
                && manualArgs[operandIndex - 1] == "--")
            {
                indicesToRemove.Add(operandIndex - 1);
                consumedOptionTerminator = true;
            }
        }

        foreach (var index in indicesToRemove.OrderDescending())
        {
            manualArgs.RemoveAt(index);
        }

        return new RequiredOperandMatch(result, materializedValues, consumedOptionTerminator);
    }

    private static ExtractedManualOptions ExtractRecognizedManualOptionsByScope(
        List<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> globalCommandModel,
        IReadOnlyList<PropertyCommandLinePart> commandSpecificModel,
        CommandLineToolOptions options,
        bool preserveTerminalOptions,
        ICollection<int>? positionalArgumentIndices = null)
    {
        var commandModel = globalCommandModel.Concat(commandSpecificModel).ToList();
        var flagsByName = commandModel
            .OfType<FlagPart>()
            .SelectMany(static part => new[]
            {
                (Name: part.Attribute.Name, Part: part),
                (Name: part.Attribute.ShortForm, Part: part),
                (Name: part.Attribute.NegatedName, Part: part),
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
        var optionParsingCount = GetOptionParsingCount(
            manualArgs,
            flagsByName,
            optionsByName,
            options);
        for (var index = 0; index < optionParsingCount;)
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
                if (manualArgs[index] != "--")
                {
                    positionalArgumentIndices?.Add(index);
                }

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

        AppendOptionTerminatedArguments(
            manualArgs,
            optionParsingCount,
            remainingArguments,
            positionalArgumentIndices);

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

    private static int GetOptionParsingCount(
        List<string> manualArgs,
        IReadOnlyDictionary<string, FlagPart> flagsByName,
        IReadOnlyDictionary<string, OptionPart> optionsByName,
        CommandLineToolOptions options)
    {
        if (!options.ArgumentsContainOptionTerminator)
        {
            return manualArgs.Count;
        }

        for (var index = 0; index < manualArgs.Count;)
        {
            if (manualArgs[index] == "--")
            {
                return index;
            }

            var match = TryMatchManualOption(
                manualArgs,
                index,
                flagsByName,
                optionsByName,
                options);
            index += match?.ArgumentCount ?? 1;
        }

        return manualArgs.Count;
    }

    private static void AppendOptionTerminatedArguments(
        List<string> manualArgs,
        int optionParsingCount,
        List<string> remainingArguments,
        ICollection<int>? positionalArgumentIndices)
    {
        remainingArguments.AddRange(manualArgs.Skip(optionParsingCount));
        for (var index = optionParsingCount + 1; index < manualArgs.Count; index++)
        {
            positionalArgumentIndices?.Add(index);
        }
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

    private readonly record struct RequiredOperandMatch(
        IReadOnlyDictionary<ArgumentPart, string> ManualValues,
        IReadOnlyDictionary<ArgumentPart, IReadOnlyList<string>> MaterializedValues,
        bool ConsumedOptionTerminator);

    private readonly record struct ManualOptionMatch(
        int ArgumentCount,
        bool IsGlobalOption,
        bool IsTerminal);
}
