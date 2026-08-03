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
/// 3. Handle placeholder replacement in command parts
/// 4. Build arguments from [CliOption], [CliFlag], and [CliArgument] attributes
/// 5. Add manual Arguments if present
/// 6. Add RunSettings after "--" if present.
/// </remarks>
internal sealed class CommandLineBuilder : ICommandLineBuilder
{
    private readonly IToolResolver _toolResolver;
    private readonly ICommandPartsProvider _commandPartsProvider;
    private readonly IPlaceholderHandler _placeholderHandler;
    private readonly ICommandModelProvider _commandModelProvider;
    private readonly ICommandArgumentBuilder _commandArgumentBuilder;

    public CommandLineBuilder(
        IToolResolver toolResolver,
        ICommandPartsProvider commandPartsProvider,
        IPlaceholderHandler placeholderHandler,
        ICommandModelProvider commandModelProvider,
        ICommandArgumentBuilder commandArgumentBuilder)
    {
        _toolResolver = toolResolver;
        _commandPartsProvider = commandPartsProvider;
        _placeholderHandler = placeholderHandler;
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

        // 2. Get subcommand parts and handle placeholder replacement
        var rawCommandParts = _commandPartsProvider.GetRawCommandParts(options);
        var precedingArgs = _placeholderHandler.ReplacePlaceholders(rawCommandParts, options);

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

        // 4. Combine: global args + preceding args (subcommands) + property args
        var allArgs = new List<string>(globalArgs);
        allArgs.AddRange(precedingArgs);
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
