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
/// 5. Add RunSettings after "--" if present.
/// </remarks>
internal sealed class CommandLineBuilder(
    IToolResolver toolResolver,
    ICommandPartsProvider commandPartsProvider,
    ICommandModelProvider commandModelProvider,
    ICommandArgumentBuilder commandArgumentBuilder) : ICommandLineBuilder
{
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
        var globalArgs = _commandArgumentBuilder.BuildArguments(globalCommandModel, options);
        var propertyArgs = _commandArgumentBuilder.BuildArguments(commandSpecificModel, options);
        var terminalArgs = _commandArgumentBuilder.BuildArguments(terminalCommandModel, options);

        // 4. Combine: global args + command parts (subcommands) + property args
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(commandParts);
        allArgs.AddRange(propertyArgs);

        // 5. Add any manual arguments passed via options.Arguments
        var manualArgs = options.Arguments?.ToList() ?? [];

        if (terminalArgs.Count > 0)
        {
            var endOfOptionsModel = commandModel
                .Where(part => part.Phase == CommandLinePhase.EndOfOptions)
                .ToList();
            var hasPropertyEndOfOptions =
                _commandArgumentBuilder.BuildArguments(endOfOptionsModel, options).Count > 0;
            var hasManualEndOfOptions = manualArgs.Contains("--", StringComparer.Ordinal);

            if (hasPropertyEndOfOptions || hasManualEndOfOptions || options.RunSettings is not null)
            {
                throw new InvalidOperationException(
                    "Terminal options cannot be combined with an end-of-options marker.");
            }
        }

        allArgs.AddRange(manualArgs);

        // 6. Add RunSettings after "--" if present
        if (options.RunSettings != null)
        {
            allArgs.Add("--");
            allArgs.AddRange(options.RunSettings);
        }

        // 7. Terminal options must follow every positional argument source.
        allArgs.AddRange(terminalArgs);

        return new CommandLine(tool, allArgs);
    }
}
