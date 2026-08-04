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
            ref emittedOptionTerminator);
        var terminatorEmittedBeforeProperties = emittedOptionTerminator;
        var propertyArgs = _commandArgumentBuilder.BuildArguments(
            commandSpecificModel,
            options,
            ref emittedOptionTerminator);
        var manualArgs = options.Arguments?.ToList() ?? [];
        // Keep recognized leading options ahead of a marker emitted by a structured argument;
        // leave manual positional operands in place after that structured argument.
        var leadingManualOptions = !terminatorEmittedBeforeProperties && emittedOptionTerminator
            ? TakeLeadingManualOptions(manualArgs, commandSpecificModel)
            : [];
        if (options.ArgumentsContainOptionTerminator
            && !manualArgs.Contains("--", StringComparer.Ordinal))
        {
            throw new ArgumentException(
                $"{nameof(CommandLineToolOptions.ArgumentsContainOptionTerminator)} requires "
                + $"{nameof(CommandLineToolOptions.Arguments)} to contain '--'.",
                nameof(options));
        }

        if (options.ArgumentsContainOptionTerminator && emittedOptionTerminator)
        {
            throw new InvalidOperationException(
                "Manual arguments cannot supply an end-of-options marker after one was already "
                + "emitted by a structured argument. Remove one of the '--' sources.");
        }

        emittedOptionTerminator |= options.ArgumentsContainOptionTerminator;
        var runSettingsArgs = _commandArgumentBuilder.BuildArguments(
            RunSettingsCommandModel,
            options,
            ref emittedOptionTerminator);
        var terminalArgumentArgs = _commandArgumentBuilder.BuildArguments(
            [.. terminalCommandModel.Where(static part => part is ArgumentPart)],
            options,
            ref emittedOptionTerminator);
        var terminalOptionArgs = _commandArgumentBuilder.BuildArguments(
            [.. terminalCommandModel.Where(static part => part is FlagPart or OptionPart)],
            options,
            ref emittedOptionTerminator);

        // 4. Combine: global args + command parts (subcommands) + property args
        // with any hoisted manual options before an emitted option terminator.
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(commandParts);
        allArgs.AddRange(leadingManualOptions);
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

    private static IReadOnlyList<string> TakeLeadingManualOptions(
        List<string> manualArgs,
        IReadOnlyList<PropertyCommandLinePart> commandModel)
    {
        var flagNames = commandModel
            .OfType<FlagPart>()
            .SelectMany(static part => new[] { part.Attribute.Name, part.Attribute.ShortForm })
            .OfType<string>()
            .ToHashSet(StringComparer.Ordinal);
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
        var count = 0;
        while (count < manualArgs.Count)
        {
            if (flagNames.Contains(manualArgs[count]))
            {
                count++;
                continue;
            }

            if (!optionsByName.TryGetValue(manualArgs[count], out var option))
            {
                break;
            }

            var operandCount = GetManualOperandCount(option);
            if (manualArgs.Count - count - 1 < operandCount)
            {
                break;
            }

            count += operandCount + 1;
        }

        if (count == 0)
        {
            return [];
        }

        var options = manualArgs.GetRange(0, count);
        manualArgs.RemoveRange(0, count);
        return options;
    }

    private static int GetManualOperandCount(OptionPart option)
    {
        if (option.Attribute.ValueArity == CliOptionValueArity.Optional)
        {
            return 0;
        }

        return option.ManualOperandCount >= 0
            ? option.ManualOperandCount
            : throw new InvalidOperationException(
                $"Manual value count cannot be negative for {option.PropertyName}.");
    }
}
