using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.Options;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

/// <summary>
/// Orchestrates command-line tool execution by coordinating argument building,
/// placeholder replacement, and command execution.
/// </summary>
internal sealed class Command : ICommand, ICommandContext
{
    private readonly ICommandLogger _commandLogger;
    private readonly ICommandModelProvider _commandModelProvider;
    private readonly ICommandArgumentBuilder _commandArgumentBuilder;
    private readonly IPlaceholderHandler _placeholderHandler;
    private readonly ICommandPartsProvider _commandPartsProvider;
    private readonly IToolResolver _toolResolver;

    public Command(
        ICommandLogger commandLogger,
        ICommandModelProvider commandModelProvider,
        ICommandArgumentBuilder commandArgumentBuilder,
        IPlaceholderHandler placeholderHandler,
        ICommandPartsProvider commandPartsProvider,
        IToolResolver toolResolver)
    {
        _commandLogger = commandLogger;
        _commandModelProvider = commandModelProvider;
        _commandArgumentBuilder = commandArgumentBuilder;
        _placeholderHandler = placeholderHandler;
        _commandPartsProvider = commandPartsProvider;
        _toolResolver = toolResolver;
    }

    public async Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        var execOpts = executionOptions ?? new CommandExecutionOptions();

        var optionsObject = GetOptionsObject(options);

        // Get subcommand parts and handle placeholder replacement
        var rawCommandParts = _commandPartsProvider.GetRawCommandParts(optionsObject);
        var precedingArgs = _placeholderHandler.ReplacePlaceholders(rawCommandParts, optionsObject);

        // Build arguments from the command model using the new services
        var commandModel = _commandModelProvider.GetCommandModel(optionsObject.GetType());
        var propertyArgs = _commandArgumentBuilder.BuildArguments(commandModel, optionsObject);

        // Combine: preceding args (subcommands) + property args
        var parsedArgs = precedingArgs.Concat(propertyArgs).ToList();

        // Add any manual arguments passed via options.Arguments
        var resolvedTool = _toolResolver.ResolveTool(options)
            ?? throw new InvalidOperationException($"No tool specified for {options.GetType().Name}. Add [CliTool] attribute to the options class.");
        var manualArgs = (string.Equals(options.Arguments?.ElementAtOrDefault(0), resolvedTool)
            ? options.Arguments?.Skip(1).ToList() : options.Arguments?.ToList()) ?? new List<string>();
        parsedArgs.AddRange(manualArgs);

        if (options.RunSettings != null)
        {
            parsedArgs.Add("--");
            parsedArgs.AddRange(options.RunSettings);
        }

        string tool;
        if (execOpts.Sudo)
        {
            tool = "sudo";
            parsedArgs.Insert(0, resolvedTool);
        }
        else
        {
            tool = resolvedTool;
        }

        var command = Cli.Wrap(tool).WithArguments(parsedArgs);

        if (execOpts.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(execOpts.WorkingDirectory);
        }

        if (execOpts.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(execOpts.EnvironmentVariables));
        }

        if (execOpts.InternalDryRun)
        {
            _commandLogger.Log(
                options: options,
                execOpts: execOpts,
                inputToLog: execOpts.InputLoggingManipulator == null ? command.ToString() : execOpts.InputLoggingManipulator(command.ToString()),
                exitCode: 0,
                runTime: TimeSpan.Zero,
                standardOutput: "Dummy Output Response",
                standardError: "Dummy Error Response",
                commandWorkingDirPath: command.WorkingDirPath
            );

            return new CommandResult(command);
        }

        return await Of(command, options, execOpts, cancellationToken).ConfigureAwait(false);
    }

    private static object GetOptionsObject(CommandLineToolOptions options)
    {
        return options;
    }

    private async Task<CommandResult> Of(
        CliWrap.Command command,
        CommandLineToolOptions options,
        CommandExecutionOptions execOpts,
        CancellationToken cancellationToken = default)
    {
        StringBuilder standardOutputStringBuilder = new();
        StringBuilder standardErrorStringBuilder = new();
        var stopwatch = Stopwatch.StartNew();

        var standardOutput = string.Empty;
        var standardError = string.Empty;

        var inputToLog = execOpts.InputLoggingManipulator == null ? command.ToString() : execOpts.InputLoggingManipulator(command.ToString());

        // Only create timeout token if ExecutionTimeout is specified to avoid unnecessary allocations
        using var timeoutCancellationToken = execOpts.ExecutionTimeout.HasValue
            ? new CancellationTokenSource(execOpts.ExecutionTimeout.Value)
            : null;

        // Link the timeout token with the passed cancellation token, or just wrap the original if no timeout
        using var linkedCancellationToken = timeoutCancellationToken != null
            ? CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationToken.Token, cancellationToken)
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        using var forcefulCancellationToken = new CancellationTokenSource();

        var registration = linkedCancellationToken.Token.Register(() =>
        {
            try
            {
                if (forcefulCancellationToken.Token.CanBeCanceled)
                {
                    forcefulCancellationToken.CancelAfter(execOpts.GracefulShutdownTimeout);
                }
            }
            catch (ObjectDisposedException)
            {
                // Ignored
            }
        });
        await using (registration.ConfigureAwait(false))
        {
            try
            {
                var result = await command
                    .WithStandardOutputPipe(PipeTarget.ToStringBuilder(standardOutputStringBuilder))
                    .WithStandardErrorPipe(PipeTarget.ToStringBuilder(standardErrorStringBuilder))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(forcefulCancellationToken.Token, linkedCancellationToken.Token).ConfigureAwait(false);

                standardOutput = execOpts.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
                standardError = execOpts.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: result.ExitCode,
                    runTime: result.RunTime,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                if (result.ExitCode != 0 && execOpts.ThrowOnNonZeroExitCode)
                {
                    var input = inputToLog;
                    throw new CommandException(input, result.ExitCode, result.RunTime, standardOutput, standardError);
                }

                return new CommandResult(command, result, standardOutput, standardError);
            }
            catch (CommandExecutionException e)
            {
                standardOutput = execOpts.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
                standardError = execOpts.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: e.ExitCode,
                    runTime: stopwatch.Elapsed,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                throw new CommandException(inputToLog, e.ExitCode, stopwatch.Elapsed, standardOutput, standardError, e);
            }
            catch (Exception e) when (e is not CommandExecutionException and not CommandException)
            {
                standardOutput = execOpts.OutputLoggingManipulator == null ? standardOutputStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardOutputStringBuilder.ToString());
                standardError = execOpts.OutputLoggingManipulator == null ? standardErrorStringBuilder.ToString() : execOpts.OutputLoggingManipulator(standardErrorStringBuilder.ToString());

                _commandLogger.Log(
                    options: options,
                    execOpts: execOpts,
                    inputToLog: inputToLog,
                    exitCode: -1,
                    runTime: stopwatch.Elapsed,
                    standardOutput: standardOutput,
                    standardError: standardError,
                    commandWorkingDirPath: command.WorkingDirPath
                );

                throw new CommandException(inputToLog, -1, stopwatch.Elapsed, standardOutput, standardError, e);
            }
        }
    }
}
