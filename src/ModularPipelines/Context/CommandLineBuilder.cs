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
internal sealed class CommandLineBuilder : ICommandLineBuilder
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

    private readonly IToolResolver _toolResolver;
    private readonly ICommandPartsProvider _commandPartsProvider;
    private readonly ICommandModelProvider _commandModelProvider;
    private readonly ICommandArgumentBuilder _commandArgumentBuilder;

    public CommandLineBuilder(
        IToolResolver toolResolver,
        ICommandPartsProvider commandPartsProvider,
        ICommandModelProvider commandModelProvider,
        ICommandArgumentBuilder commandArgumentBuilder)
    {
        _toolResolver = toolResolver;
        _commandPartsProvider = commandPartsProvider;
        _commandModelProvider = commandModelProvider;
        _commandArgumentBuilder = commandArgumentBuilder;
    }

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
        var propertyArgs = _commandArgumentBuilder.BuildArguments(
            commandSpecificModel,
            options,
            ref emittedOptionTerminator);
        var emittedOptionTerminatorBeforeTerminal = emittedOptionTerminator;
        var terminalArgs = _commandArgumentBuilder.BuildArguments(
            terminalCommandModel,
            options,
            ref emittedOptionTerminator);
        var terminalArgsEmittedOptionTerminator = emittedOptionTerminator
                                                  && !emittedOptionTerminatorBeforeTerminal;
        var runSettingsArgs = _commandArgumentBuilder.BuildArguments(
            RunSettingsCommandModel,
            options,
            ref emittedOptionTerminator);

        // 4. Combine: global args + command parts (subcommands) + property args
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(commandParts);
        allArgs.AddRange(propertyArgs);

        // 5. Add any manual arguments passed via options.Arguments
        // Skip the tool name if it appears as the first argument (backward compatibility)
        var manualArgs = options.Arguments?.ToList() ?? new List<string>();
        if (manualArgs.Count > 0 && string.Equals(manualArgs[0], tool, StringComparison.Ordinal))
        {
            manualArgs = manualArgs.Skip(1).ToList();
        }

        allArgs.AddRange(manualArgs);

        // 6. Render RunSettings as option-terminated pass-through arguments.
        allArgs.AddRange(runSettingsArgs);

        // 7. A terminal option must not follow any rendered or manually supplied option terminator.
        var hasOptionTerminator = emittedOptionTerminatorBeforeTerminal
                                  || (!terminalArgsEmittedOptionTerminator && emittedOptionTerminator)
                                  || (terminalArgsEmittedOptionTerminator && runSettingsArgs.Count > 0)
                                  || manualArgs.Contains("--", StringComparer.Ordinal);
        if (terminalArgs.Count > 0 && hasOptionTerminator)
        {
            throw new InvalidOperationException(
                "Terminal options cannot be combined with arguments that emit or supply an "
                + "end-of-options marker. Remove either the terminal option or the '--' source.");
        }

        // Terminal options must follow every positional argument source.
        allArgs.AddRange(terminalArgs);

        return new CommandLine(tool, allArgs);
    }
}
