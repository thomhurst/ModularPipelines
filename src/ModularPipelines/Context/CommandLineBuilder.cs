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
/// 2. Get subcommand parts from [CliCommand] or [CliSubCommand] attributes
/// 3. Handle placeholder replacement in command parts
/// 4. Build arguments from [CliOption], [CliFlag], and [CliArgument] attributes
/// 5. Add manual Arguments if present
/// 6. Add RunSettings after "--" if present
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

        // 3. Build arguments from properties using the command model
        var commandModel = _commandModelProvider.GetCommandModel(options.GetType());
        var propertyArgs = _commandArgumentBuilder.BuildArguments(commandModel, options);

        // 4. Combine: preceding args (subcommands) + property args
        var allArgs = new List<string>(precedingArgs);
        allArgs.AddRange(propertyArgs);

        // 5. Add any manual arguments passed via options.Arguments
        // Skip the tool name if it appears as the first argument (backward compatibility)
        var manualArgs = options.Arguments?.ToList() ?? new List<string>();
        if (manualArgs.Count > 0 && string.Equals(manualArgs[0], tool, StringComparison.Ordinal))
        {
            manualArgs = manualArgs.Skip(1).ToList();
        }

        allArgs.AddRange(manualArgs);

        // 6. Add RunSettings after "--" if present
        if (options.RunSettings != null)
        {
            allArgs.Add("--");
            allArgs.AddRange(options.RunSettings);
        }

        return new CommandLine(tool, allArgs);
    }
}
