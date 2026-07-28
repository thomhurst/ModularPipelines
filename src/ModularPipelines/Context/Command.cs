using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
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
    private readonly ICommandLineBuilder _commandLineBuilder;
    private readonly ISecretProvider _secretProvider;
    private readonly ISecretRegistry _secretRegistry;

    public Command(
        ICommandLogger commandLogger,
        ICommandLineBuilder commandLineBuilder,
        ISecretProvider secretProvider,
        ISecretRegistry secretRegistry)
    {
        _commandLogger = commandLogger;
        _commandLineBuilder = commandLineBuilder;
        _secretProvider = secretProvider;
        _secretRegistry = secretRegistry;
    }

    public async Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        var execOpts = executionOptions ?? new CommandExecutionOptions();

        _secretRegistry.AddSecrets(_secretProvider.GetSecretsInObject(options));

        var commandLine = _commandLineBuilder.Build(options);
        var resolvedTool = commandLine.Tool;
        var parsedArgs = commandLine.Arguments.ToList();

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

        var command = CliCommandFactory.Create(tool, parsedArgs, execOpts);

        if (execOpts.WorkingDirectory != null)
        {
            command = command.WithWorkingDirectory(execOpts.WorkingDirectory);
        }

        if (execOpts.CommandLineCredentials != null)
        {
            command = command.WithCredentials(execOpts.CommandLineCredentials.ToCliWrapCredentials());
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
        var processTreeTerminator = new ProcessTreeTerminator();
        using var processTreeCancellationRegistration =
            forcefulCancellationToken.Token.Register(processTreeTerminator.Kill);

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
                    .ExecuteAsync(
                        configureStartInfo: startInfo =>
                        {
                            if (OperatingSystem.IsWindows())
                            {
                                startInfo.CreateNewProcessGroup = true;
                            }
                        },
                        configureProcess: processTreeTerminator.Attach,
                        forcefulCancellationToken: CancellationToken.None,
                        gracefulCancellationToken: linkedCancellationToken.Token)
                    .ConfigureAwait(false);

                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

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
                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

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
                standardOutput = standardOutputStringBuilder.ToString();
                standardError = standardErrorStringBuilder.ToString();

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

    private sealed class ProcessTreeTerminator
    {
        private readonly Lock _lock = new();
        private Process? _process;
        private bool _killRequested;

        public void Attach(Process process)
        {
            lock (_lock)
            {
                _process = process;

                if (!_killRequested)
                {
                    return;
                }
            }

            TryKill(process);
        }

        public void Kill()
        {
            Process? process;

            lock (_lock)
            {
                _killRequested = true;
                process = _process;
            }

            if (process is not null)
            {
                TryKill(process);
            }
        }

        private static void TryKill(Process process)
        {
            try
            {
                process.Kill(entireProcessTree: true);
            }
            catch (InvalidOperationException)
            {
                // The process already exited.
            }
            catch (Win32Exception)
            {
                // The process already exited or could not be terminated.
            }
            catch (NotSupportedException)
            {
                // The process is remote.
            }
        }
    }
}
