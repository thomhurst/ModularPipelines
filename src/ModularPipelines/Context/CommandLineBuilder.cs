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
/// 4. Insert phase-aware AdditionalArguments and append manual Arguments
/// 5. Add RunSettings after "--" and terminal options last.
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
        var terminalArgs = _commandArgumentBuilder.BuildArguments(terminalCommandModel, options)
            .Concat(GetAdditionalArguments(additionalArguments, CommandLinePhase.Terminal))
            .ToList();

        // 4. Combine: global args + command parts (subcommands) + command-specific args.
        var allArgs = new List<string>();
        AddNonTerminalArguments(allArgs, globalCommandModel, additionalArguments, options, isGlobalOption: true);
        allArgs.AddRange(commandParts);
        AddNonTerminalArguments(allArgs, commandSpecificModel, additionalArguments, options, isGlobalOption: false);

        // 5. Add any manual arguments passed via options.Arguments
        var manualArgs = options.Arguments?.ToList() ?? [];

        if (terminalArgs.Count > 0)
        {
            var endOfOptionsModel = commandModel
                .Where(part => part.Phase == CommandLinePhase.EndOfOptions)
                .ToList();
            var hasPropertyEndOfOptions =
                _commandArgumentBuilder.BuildArguments(endOfOptionsModel, options).Count > 0;
            var hasAdditionalEndOfOptions = additionalArguments
                .Any(argument => argument.Phase == CommandLinePhase.EndOfOptions);
            var hasManualEndOfOptions = manualArgs.Contains("--", StringComparer.Ordinal);

            if (hasPropertyEndOfOptions
                || hasAdditionalEndOfOptions
                || hasManualEndOfOptions
                || options.RunSettings is not null)
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

    private void AddNonTerminalArguments(
        List<string> destination,
        IReadOnlyList<PropertyCommandLinePart> commandModel,
        IReadOnlyList<AdditionalCommandLineArgument> additionalArguments,
        CommandLineToolOptions options,
        bool isGlobalOption)
    {
        foreach (var phase in Enum.GetValues<CommandLinePhase>()
                     .Where(phase => phase != CommandLinePhase.Terminal))
        {
            var phaseModel = commandModel.Where(part => part.Phase == phase).ToList();
            destination.AddRange(GetAdditionalArguments(additionalArguments, phase, isGlobalOption));
            destination.AddRange(_commandArgumentBuilder.BuildArguments(phaseModel, options));
        }
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
        IEnumerable<AdditionalCommandLineArgument> additionalArguments)
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
        }
    }
}
