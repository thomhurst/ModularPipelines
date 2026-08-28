using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using CliWrap;
using CliWrap.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using ModularPipelines.Constants;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Tracing;
using CommandResult = ModularPipelines.Models.CommandResult;

namespace ModularPipelines.Context;

/// <summary>
/// Orchestrates command-line tool execution by coordinating argument building,
/// placeholder replacement, and command execution.
/// </summary>
internal sealed class Command : ICommandContext
{
    // Win32 ERROR_FILE_NOT_FOUND and Unix ENOENT both use native error code 2.
    private const int FileNotFoundNativeErrorCode = 2;

    private sealed record PreparedCommandInvocation(
        CommandLine CommandLine,
        CommandLineToolOptions ToolOptions,
        CommandExecutionOptions ExecutionOptions,
        string RawCommandInput,
        string WorkingDirectory,
        IReadOnlyDictionary<string, string?> RawEnvironmentVariables);

    private readonly ICommandLogger _commandLogger;
    private readonly ICommandLineBuilder _commandLineBuilder;
    private readonly IEnumerable<ICommandInterceptor> _commandInterceptors;
    private readonly ISecretProvider _secretProvider;
    private readonly ISecretRegistry _secretRegistry;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ICommandExecutionCounter _commandExecutionCounter;
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly PipelineWorkingDirectory _pipelineWorkingDirectory;

    public Command(
        ICommandLogger commandLogger,
        ICommandLineBuilder commandLineBuilder,
        IEnumerable<ICommandInterceptor> commandInterceptors,
        ISecretProvider secretProvider,
        ISecretRegistry secretRegistry,
        ISecretObfuscator secretObfuscator,
        ICommandExecutionCounter commandExecutionCounter,
        IOptions<PipelineOptions> pipelineOptions,
        PipelineWorkingDirectory pipelineWorkingDirectory)
    {
        _commandLogger = commandLogger;
        _commandLineBuilder = commandLineBuilder;
        _commandInterceptors = commandInterceptors;
        _secretProvider = secretProvider;
        _secretRegistry = secretRegistry;
        _secretObfuscator = secretObfuscator;
        _commandExecutionCounter = commandExecutionCounter;
        _pipelineOptions = pipelineOptions;
        _pipelineWorkingDirectory = pipelineWorkingDirectory;
    }

    public async Task<CommandResult> ExecuteCommandLineToolAsync(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        var execOpts = (executionOptions ?? new CommandExecutionOptions()) with
        {
            WorkingDirectory = executionOptions?.WorkingDirectory is { } workingDirectory
                ? _pipelineWorkingDirectory.ResolvePath(workingDirectory)
                : _pipelineWorkingDirectory.Path,
        };
        RegisterSecrets(options, execOpts);
        (CliWrap.Command Command, string CommandInput, string Tool, List<string> ParsedArgs) commandDetails;
        try
        {
            commandDetails = CreateCommand(options, execOpts);
        }
        catch (Exception exception)
        {
            using var creationActivity = ModuleActivityTracing.StartCommandActivity(options.GetType().Name);
            ModuleActivityTracing.RecordCommandFailure(
                creationActivity,
                exception,
                _secretObfuscator.Obfuscate(exception.Message, execOpts));
            throw;
        }

        _commandExecutionCounter.Record(AmbientModuleContext.CurrentModuleType);
        var (command, commandInput, tool, parsedArgs) = commandDetails;

        cancellationToken.ThrowIfCancellationRequested();

        var rawEnvironmentVariables = GetRawEnvironmentVariables(command);
        var invocation = new PreparedCommandInvocation(
            new CommandLine(tool, parsedArgs),
            options,
            execOpts,
            commandInput,
            command.WorkingDirPath,
            rawEnvironmentVariables);

        using var timeoutCancellationToken = CreateTimeoutCancellationToken(execOpts);
        using var linkedCancellationToken =
            CreateLinkedCancellationToken(timeoutCancellationToken, cancellationToken);
        var obfuscatedTool = _secretObfuscator.Obfuscate(tool, execOpts);
        using var activity = ModuleActivityTracing.StartCommandActivity(obfuscatedTool);
        var inputToLog = new Lazy<string>(() =>
        {
            var input = GetInputToLog(commandInput, execOpts);
            RecordTelemetryCommandInput(activity, input, execOpts);
            return input;
        });

        try
        {
            var result = await ExecuteCommandCoreAsync(
                    invocation,
                    command,
                    commandInput,
                    options,
                    execOpts,
                    inputToLog,
                    linkedCancellationToken.Token,
                    cancellationToken,
                    timeoutCancellationToken)
                .ConfigureAwait(false);
            ModuleActivityTracing.RecordCommandResult(activity, result);
            return result;
        }
        catch (OperationCanceledException exception)
            when (!cancellationToken.IsCancellationRequested
                  && timeoutCancellationToken?.IsCancellationRequested is true)
        {
            var timeoutException = CreateTimeoutException(execOpts, exception);
            ModuleActivityTracing.RecordCommandFailure(
                activity,
                timeoutException,
                _secretObfuscator.Obfuscate(timeoutException.Message, execOpts));
            throw timeoutException;
        }
        catch (Exception exception)
        {
            ModuleActivityTracing.RecordCommandFailure(
                activity,
                exception,
                _secretObfuscator.Obfuscate(exception.Message, execOpts));
            throw;
        }
    }

    private void RegisterSecrets(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions)
    {
        _secretRegistry.AddSecrets(_secretProvider.GetSecretsInObject(options));
        _secretRegistry.AddSecrets(_secretProvider.GetSecretsInObject(executionOptions));
        _secretRegistry.AddSecrets(_secretProvider.GetSecretsInObject(executionOptions.CommandLineCredentials));
    }

    private (CliWrap.Command Command, string CommandInput, string Tool, List<string> Arguments) CreateCommand(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions)
    {
        var commandLine = _commandLineBuilder.Build(options);
        var tool = commandLine.Tool;
        var arguments = commandLine.Arguments.ToList();
        if (executionOptions.Sudo)
        {
            arguments.Insert(0, tool);
            tool = "sudo";
        }

        var preparedCommand = CliCommandFactory.Create(tool, arguments, executionOptions);
        var command = preparedCommand.Command;
        if (executionOptions.WorkingDirectory is not null)
        {
            command = command.WithWorkingDirectory(executionOptions.WorkingDirectory);
        }

        if (executionOptions.CommandLineCredentials is not null)
        {
            command = command.WithCredentials(executionOptions.CommandLineCredentials.ToCliWrapCredentials());
        }

        return (command, preparedCommand.Input, tool, arguments);
    }

    private async Task<CommandResult> ExecuteCommandCoreAsync(
        PreparedCommandInvocation invocation,
        CliWrap.Command command,
        string commandInput,
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        Lazy<string> inputToLog,
        CancellationToken executionCancellationToken,
        CancellationToken callerCancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        executionCancellationToken.ThrowIfCancellationRequested();

        var intercepted = await TryInterceptAsync(
                invocation,
                command,
                options,
                executionOptions,
                inputToLog,
                executionCancellationToken)
            .ConfigureAwait(false);
        if (intercepted is not null)
        {
            return intercepted;
        }

        return executionOptions.InternalDryRun
            ? ExecuteDryRun(
                command,
                commandInput,
                options,
                executionOptions,
                inputToLog,
                invocation.RawEnvironmentVariables)
            : await Of(
                    command,
                    commandInput,
                    options,
                    executionOptions,
                    inputToLog,
                    invocation.RawEnvironmentVariables,
                    executionCancellationToken,
                    callerCancellationToken,
                    timeoutCancellationToken)
                .ConfigureAwait(false);
    }

    private void RecordTelemetryCommandInput(
        Activity? activity,
        string inputToLog,
        CommandExecutionOptions executionOptions)
    {
        if (activity is null)
        {
            return;
        }

        ModuleActivityTracing.RecordCommandInput(
            activity,
            GetTelemetryCommandInput(inputToLog, executionOptions));
    }

    private async Task<CommandResult?> TryInterceptAsync(
        PreparedCommandInvocation invocation,
        CliWrap.Command command,
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        Lazy<string> inputToLog,
        CancellationToken cancellationToken)
    {
        CommandInvocation? publicInvocation = null;
        var publicInvocationSecretVersion = long.MinValue;
        foreach (var interceptor in _commandInterceptors)
        {
            if (publicInvocation is null
                || publicInvocationSecretVersion != _secretProvider.Version)
            {
                (publicInvocation, publicInvocationSecretVersion) =
                    CreatePublicInvocation(invocation, executionOptions);
            }

            var intercepted = await interceptor
                .InterceptAsync(publicInvocation, cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (intercepted is null)
            {
                continue;
            }

            var result = ApplyCommandMetadata(
                intercepted,
                command,
                invocation,
                executionOptions);
            LogInterceptedCommand(options, executionOptions, inputToLog.Value, result);
            if (result.ExitCode != 0 && executionOptions.ThrowOnNonZeroExitCode)
            {
                throw CommandException.FromAlreadyObfuscatedResult(CreateFailureResult(
                    command,
                    executionOptions,
                    result.CommandInput,
                    result.ExitCode,
                    result.Duration,
                    result.StandardOutput,
                    result.StandardError,
                    invocation.RawEnvironmentVariables,
                    result.StartTime,
                    result.EndTime));
            }

            return result;
        }

        return null;
    }

    private (CommandInvocation Invocation, long SecretVersion) CreatePublicInvocation(
        PreparedCommandInvocation invocation,
        CommandExecutionOptions executionOptions)
    {
        var (commandInput, environmentVariables, secretVersion) =
            CreatePublicCommandMetadata(invocation, executionOptions);

        return (new CommandInvocation(
            invocation.CommandLine,
            invocation.ToolOptions,
            invocation.ExecutionOptions,
            commandInput,
            invocation.WorkingDirectory,
            environmentVariables), secretVersion);
    }

    private (string CommandInput, IReadOnlyDictionary<string, string?> EnvironmentVariables, long SecretVersion)
        CreatePublicCommandMetadata(
            PreparedCommandInvocation invocation,
            CommandExecutionOptions executionOptions)
    {
        while (true)
        {
            var versionBefore = _secretProvider.Version;
            if ((versionBefore & 1) != 0)
            {
                Thread.Yield();
                continue;
            }

            var commandInput = _secretObfuscator.Obfuscate(
                invocation.RawCommandInput,
                executionOptions);
            var environmentVariables = ObfuscateEnvironmentVariables(
                invocation.RawEnvironmentVariables,
                executionOptions);
            if (_secretProvider.Version != versionBefore)
            {
                continue;
            }

            return (commandInput, environmentVariables, versionBefore);
        }
    }

    private CommandResult ExecuteDryRun(
        CliWrap.Command command,
        string commandInput,
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        Lazy<string> inputToLog,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables)
    {
        _commandLogger.Log(
            options: options,
            execOpts: executionOptions,
            inputToLog: inputToLog.Value,
            exitCode: 0,
            runTime: TimeSpan.Zero,
            standardOutput: "Dummy Output Response",
            standardError: "Dummy Error Response",
            commandWorkingDirPath: command.WorkingDirPath);

        return new CommandResult(
            command,
            _secretObfuscator.Obfuscate(commandInput, executionOptions),
            ObfuscateEnvironmentVariables(rawEnvironmentVariables, executionOptions));
    }

    private CommandResult ApplyCommandMetadata(
        CommandResult result,
        CliWrap.Command command,
        PreparedCommandInvocation invocation,
        CommandExecutionOptions executionOptions)
    {
        var (commandInput, environmentVariables, _) =
            CreatePublicCommandMetadata(invocation, executionOptions);

        return result with
        {
            CommandInput = commandInput,
            WorkingDirectory = command.WorkingDirPath,
            EnvironmentVariables = environmentVariables,
        };
    }

    private void LogInterceptedCommand(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string inputToLog,
        CommandResult result)
    {
        _commandLogger.Log(
            options,
            executionOptions,
            inputToLog,
            result.ExitCode,
            result.Duration,
            result.StandardOutput,
            result.StandardError,
            result.WorkingDirectory);
    }

    private async Task<CommandResult> Of(
        CliWrap.Command command,
        string commandInput,
        CommandLineToolOptions options,
        CommandExecutionOptions execOpts,
        Lazy<string> lazyInputToLog,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        CancellationToken executionCancellationToken,
        CancellationToken callerCancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        var standardOutputBuffer = new BoundedCommandOutputBuffer(execOpts.MaxCapturedOutputLength);
        var standardErrorBuffer = new BoundedCommandOutputBuffer(execOpts.MaxCapturedOutputLength);
        var completeStandardOutputBuffer = CreateCompleteOutputBuffer(execOpts);
        var completeStandardErrorBuffer = CreateCompleteOutputBuffer(execOpts);
        var outputLogger = _commandLogger as ICommandOutputLogger;
        using var deferredOutputLogger = CreateDeferredOutputLogger(
            outputLogger,
            options,
            execOpts);
        var stopwatch = Stopwatch.StartNew();

        var standardOutput = string.Empty;
        var standardError = string.Empty;

        var inputToLog = lazyInputToLog.Value;
        var loggingFailures = new DeferredCommandLoggingFailures();

        using var forcefulCancellationToken = new CancellationTokenSource();
        using var processTreeTerminator = new ProcessTreeTerminator();
        using var processTreeCancellationRegistration =
            forcefulCancellationToken.Token.Register(processTreeTerminator.Kill);

        var registration = executionCancellationToken.Register(
            () => ScheduleForcefulCancellation(
                forcefulCancellationToken,
                execOpts.GracefulShutdownTimeout,
                execOpts.InternalForcefulCancellationReady));
        loggingFailures.Capture(
            () => _commandLogger.LogCommandStart(
                options,
                execOpts,
                inputToLog,
                command.WorkingDirPath));
        await using (registration.ConfigureAwait(false))
        {
            CliWrap.CommandResult result;
            try
            {
                var executionTask = command
                    .WithStandardOutputPipe(CreateOutputTarget(
                        standardOutputBuffer,
                        completeStandardOutputBuffer,
                        CreateStandardOutputLogger(
                            deferredOutputLogger)))
                    .WithStandardErrorPipe(CreateOutputTarget(
                        standardErrorBuffer,
                        completeStandardErrorBuffer,
                        CreateStandardErrorLogger(
                            deferredOutputLogger)))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(
                        configureProcessStartInfo: ConfigureStartInfo,
                        configureProcess: processTreeTerminator.Attach,
                        forcefulCancellationToken: CancellationToken.None,
                        gracefulCancellationToken: executionCancellationToken);
                using var descendantCaptureRegistration =
                    executionCancellationToken.Register(processTreeTerminator.BeginGracefulShutdown);
                result = await executionTask.ConfigureAwait(false);

                await WaitForForcefulCancellationAsync(
                    processTreeTerminator,
                    executionCancellationToken,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputBuffer.ToString();
                standardError = standardErrorBuffer.ToString();
            }
            catch (CommandExecutionException e)
            {
                await WaitForForcefulCancellationAsync(
                    processTreeTerminator,
                    executionCancellationToken,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputBuffer.ToString();
                standardError = standardErrorBuffer.ToString();

                loggingFailures.Capture(
                    () => LogCommandCompletion(
                        options,
                        execOpts,
                        inputToLog,
                        e.ExitCode,
                        stopwatch.Elapsed,
                        standardOutput,
                        standardError,
                        completeStandardOutputBuffer,
                        completeStandardErrorBuffer,
                        deferredOutputLogger,
                        command.WorkingDirPath));
                var failure = loggingFailures.CombineWith(e);

                throw CommandException.FromAlreadyObfuscatedResult(
                    CreateFailureResult(
                        command,
                        execOpts,
                        commandInput,
                        e.ExitCode,
                        stopwatch.Elapsed,
                        standardOutput,
                        standardError,
                        rawEnvironmentVariables),
                    failure);
            }
            catch (Exception e) when (e is not CommandExecutionException and not CommandException)
            {
                await WaitForForcefulCancellationAsync(
                    processTreeTerminator,
                    executionCancellationToken,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = standardOutputBuffer.ToString();
                standardError = standardErrorBuffer.ToString();

                loggingFailures.Capture(
                    () => LogCommandCompletion(
                        options,
                        execOpts,
                        inputToLog,
                        -1,
                        stopwatch.Elapsed,
                        standardOutput,
                        standardError,
                        completeStandardOutputBuffer,
                        completeStandardErrorBuffer,
                        deferredOutputLogger,
                        command.WorkingDirPath));
                var failure = loggingFailures.CombineWith(e);

                ThrowCallerCancellationIfRequired(e, failure, callerCancellationToken);

                throw CreateExecutionFailure(
                    e,
                    failure,
                    command,
                    execOpts,
                    commandInput,
                    stopwatch.Elapsed,
                    standardOutput,
                    standardError,
                    rawEnvironmentVariables,
                    callerCancellationToken,
                    timeoutCancellationToken);
            }

            var commandFailure = CreateCommandFailure(
                command,
                result,
                execOpts,
                commandInput,
                standardOutput,
                standardError,
                rawEnvironmentVariables);

            loggingFailures.Capture(
                () => LogCommandCompletion(
                    options,
                    execOpts,
                    inputToLog,
                    result.ExitCode,
                    result.RunTime,
                    standardOutput,
                    standardError,
                    completeStandardOutputBuffer,
                    completeStandardErrorBuffer,
                    deferredOutputLogger,
                    command.WorkingDirPath));
            if (commandFailure is not null && loggingFailures.HasFailures)
            {
                throw CommandException.FromAlreadyObfuscatedResult(
                    commandFailure.Result,
                    loggingFailures.CombineWith(commandFailure));
            }

            if (commandFailure is not null)
            {
                throw commandFailure;
            }

            loggingFailures.Throw();
            return new CommandResult(
                command,
                result,
                _secretObfuscator.Obfuscate(commandInput, execOpts),
                standardOutput,
                standardError,
                ObfuscateEnvironmentVariables(rawEnvironmentVariables, execOpts));
        }
    }

    private static bool ShouldPreserveCallerCancellation(
        Exception executionFailure,
        Exception combinedFailure,
        CancellationToken cancellationToken)
    {
        return executionFailure is OperationCanceledException
               && cancellationToken.IsCancellationRequested
               && ReferenceEquals(combinedFailure, executionFailure);
    }

    private static void ThrowCallerCancellationIfRequired(
        Exception executionFailure,
        Exception combinedFailure,
        CancellationToken callerCancellationToken)
    {
        if (!ShouldPreserveCallerCancellation(
                executionFailure,
                combinedFailure,
                callerCancellationToken))
        {
            return;
        }

        if (executionFailure is OperationCanceledException cancellationException
            && cancellationException.CancellationToken != callerCancellationToken)
        {
            throw cancellationException is TaskCanceledException
                ? new TaskCanceledException(
                    cancellationException.Message,
                    cancellationException,
                    callerCancellationToken)
                : new OperationCanceledException(
                    cancellationException.Message,
                    cancellationException,
                    callerCancellationToken);
        }

        System.Runtime.ExceptionServices.ExceptionDispatchInfo
            .Capture(executionFailure)
            .Throw();
    }

    private Exception CreateExecutionFailure(
        Exception executionFailure,
        Exception combinedFailure,
        CliWrap.Command command,
        CommandExecutionOptions execOpts,
        string input,
        TimeSpan duration,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        CancellationToken cancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        if (executionFailure is OperationCanceledException
            && !cancellationToken.IsCancellationRequested
            && timeoutCancellationToken?.IsCancellationRequested is true)
        {
            return CreateTimeoutException(execOpts, combinedFailure);
        }

        var result = CreateFailureResult(
            command,
            execOpts,
            input,
            -1,
            duration,
            standardOutput,
            standardError,
            rawEnvironmentVariables);
        return IsExecutableNotFound(executionFailure)
            ? new ToolNotFoundException(
                _secretObfuscator.Obfuscate(command.TargetFilePath, execOpts),
                result,
                combinedFailure)
            : CommandException.FromAlreadyObfuscatedResult(result, combinedFailure);
    }

    private static bool IsExecutableNotFound(Exception exception)
    {
        if (exception is Win32Exception { NativeErrorCode: FileNotFoundNativeErrorCode })
        {
            return true;
        }

        if (exception is AggregateException aggregateException)
        {
            return aggregateException.InnerExceptions.Any(IsExecutableNotFound);
        }

        return exception.InnerException is not null
               && IsExecutableNotFound(exception.InnerException);
    }

    private static TimeoutException CreateTimeoutException(
        CommandExecutionOptions executionOptions,
        Exception innerException) =>
        new(
            $"Command execution timed out after {executionOptions.ExecutionTimeout!.Value}.",
            innerException);

    private CommandResult CreateFailureResult(
        CliWrap.Command command,
        CommandExecutionOptions execOpts,
        string input,
        int exitCode,
        TimeSpan duration,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        DateTimeOffset? startTime = null,
        DateTimeOffset? endTime = null)
    {
        var completedAt = endTime ?? DateTimeOffset.UtcNow;
        var startedAt = startTime ?? completedAt - duration;
        return new CommandResult(
            commandInput: _secretObfuscator.Obfuscate(input, execOpts),
            workingDirectory: command.WorkingDirPath,
            standardOutput: _secretObfuscator.Obfuscate(standardOutput, execOpts),
            standardError: _secretObfuscator.Obfuscate(standardError, execOpts),
            environmentVariables: ObfuscateEnvironmentVariables(rawEnvironmentVariables, execOpts),
            startTime: startedAt,
            endTime: completedAt,
            duration: duration,
            exitCode: exitCode);
    }

    private static Dictionary<string, string?> GetRawEnvironmentVariables(CliWrap.Command command)
    {
        return command.EnvironmentVariables
            .Where(pair => !CliCommandFactory.IsInternalEnvironmentVariable(pair.Key))
            .ToDictionary(
                pair => pair.Key,
                pair => pair.Value,
                OperatingSystem.IsWindows()
                    ? StringComparer.OrdinalIgnoreCase
                    : StringComparer.Ordinal);
    }

    private Dictionary<string, string?> ObfuscateEnvironmentVariables(
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        CommandExecutionOptions executionOptions)
    {
        return rawEnvironmentVariables.ToDictionary(
            pair => pair.Key,
            pair => pair.Value is null
                ? null
                : _secretObfuscator.Obfuscate(pair.Value, executionOptions),
            OperatingSystem.IsWindows()
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal);
    }

    private CommandException? CreateCommandFailure(
        CliWrap.Command command,
        CliWrap.CommandResult result,
        CommandExecutionOptions execOpts,
        string input,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables)
    {
        return result.ExitCode != 0 && execOpts.ThrowOnNonZeroExitCode
            ? CommandException.FromAlreadyObfuscatedResult(CreateFailureResult(
                command,
                execOpts,
                input,
                result.ExitCode,
                result.RunTime,
                standardOutput,
                standardError,
                rawEnvironmentVariables,
                result.StartTime,
                result.ExitTime))
            : null;
    }

    private static BoundedCommandOutputBuffer? CreateCompleteOutputBuffer(CommandExecutionOptions options)
    {
        return options.OutputLoggingManipulator is null
            ? null
            : new BoundedCommandOutputBuffer(maximumLength: 0);
    }

    private static string GetInputToLog(string commandInput, CommandExecutionOptions options)
    {
        return options.InputLoggingManipulator is null
            ? commandInput
            : options.InputLoggingManipulator(commandInput);
    }

    private string GetTelemetryCommandInput(string inputToLog, CommandExecutionOptions options)
    {
        var loggingOptions = options.Logging
                             ?? _pipelineOptions.Value.Commands.Logging
                             ?? CommandLoggingOptions.Default;
        return loggingOptions.Verbosity == CommandLogVerbosity.Silent
               || !loggingOptions.ShowCommandArguments
            ? LoggingConstants.CommandMask
            : _secretObfuscator.Obfuscate(inputToLog, options);
    }

    private static CancellationTokenSource? CreateTimeoutCancellationToken(CommandExecutionOptions options)
    {
        return options.ExecutionTimeout.HasValue
            ? new CancellationTokenSource(options.ExecutionTimeout.Value)
            : null;
    }

    private static CancellationTokenSource CreateLinkedCancellationToken(
        CancellationTokenSource? timeoutCancellationToken,
        CancellationToken cancellationToken)
    {
        return timeoutCancellationToken is null
            ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
            : CancellationTokenSource.CreateLinkedTokenSource(timeoutCancellationToken.Token, cancellationToken);
    }

    private static void ScheduleForcefulCancellation(
        CancellationTokenSource forcefulCancellationToken,
        TimeSpan gracefulShutdownTimeout,
        Task? forcefulCancellationReady)
    {
        _ = ScheduleForcefulCancellationAsync(
            forcefulCancellationToken,
            gracefulShutdownTimeout,
            forcefulCancellationReady);
    }

    internal static async Task ScheduleForcefulCancellationAsync(
        CancellationTokenSource forcefulCancellationToken,
        TimeSpan gracefulShutdownTimeout,
        Task? forcefulCancellationReady)
    {
        try
        {
            if (forcefulCancellationReady is not null)
            {
                try
                {
                    await forcefulCancellationReady.ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    Trace.TraceError(
                        "Forceful cancellation readiness failed; arming the timer anyway: {0}",
                        exception);
                }
            }

            if (forcefulCancellationToken.Token.CanBeCanceled)
            {
                forcefulCancellationToken.CancelAfter(gracefulShutdownTimeout);
            }
        }
        catch (ObjectDisposedException)
        {
            // Ignored
        }
    }

    private static DeferredCommandOutputLogger? CreateDeferredOutputLogger(
        ICommandOutputLogger? outputLogger,
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions)
    {
        return outputLogger is not null && executionOptions.OutputLoggingManipulator is null
            ? new DeferredCommandOutputLogger(outputLogger, options, executionOptions)
            : null;
    }

    private static Action<string>? CreateStandardOutputLogger(
        DeferredCommandOutputLogger? outputLogger)
    {
        return outputLogger is null
            ? null
            : outputLogger.LogStandardOutputLine;
    }

    private static Action<string>? CreateStandardErrorLogger(
        DeferredCommandOutputLogger? outputLogger)
    {
        return outputLogger is null
            ? null
            : outputLogger.LogStandardErrorLine;
    }

    private static void ConfigureStartInfo(ProcessStartInfo startInfo)
    {
        if (OperatingSystem.IsWindows())
        {
            startInfo.CreateNewProcessGroup = true;
        }
    }

    private static async Task WaitForForcefulCancellationAsync(
        ProcessTreeTerminator processTreeTerminator,
        CancellationToken gracefulCancellationToken,
        CancellationToken forcefulCancellationToken)
    {
        if (!gracefulCancellationToken.IsCancellationRequested ||
            forcefulCancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            while (processTreeTerminator.HasRunningProcesses())
            {
                await Task.Delay(TimeSpan.FromMilliseconds(10), forcefulCancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (forcefulCancellationToken.IsCancellationRequested)
        {
            // The graceful shutdown window elapsed.
        }
    }

    private sealed class ProcessTreeTerminator : IDisposable
    {
        private static readonly TimeSpan DescendantPollingInterval = TimeSpan.FromMilliseconds(10);
        private readonly Lock _lock = new();
        private readonly Dictionary<int, Process> _descendants = [];
        private Process? _process;
        private SafeFileHandle? _windowsJob;
        private Timer? _descendantCaptureTimer;
        private bool _disposed;
        private bool _killRequested;

        public void Attach(Process process)
        {
            var windowsJob = OperatingSystem.IsWindows() ? TryCreateWindowsJob(process) : null;

            lock (_lock)
            {
                _process = process;
                _windowsJob = windowsJob;

                if (!_killRequested)
                {
                    return;
                }
            }

            TryKill(process, windowsJob);
        }

        public void BeginGracefulShutdown()
        {
            if (OperatingSystem.IsWindows())
            {
                return;
            }

            CaptureDescendants();

            lock (_lock)
            {
                if (_disposed || _killRequested)
                {
                    return;
                }

                _descendantCaptureTimer ??= new Timer(
                    static state => ((ProcessTreeTerminator) state!).CaptureDescendants(),
                    this,
                    DescendantPollingInterval,
                    DescendantPollingInterval);
            }
        }

        public bool HasRunningProcesses()
        {
            Process? process;
            SafeFileHandle? windowsJob;
            Process[] descendants;

            lock (_lock)
            {
                process = _process;
                windowsJob = _windowsJob;
                descendants = [.. _descendants.Values];
            }

            if (OperatingSystem.IsWindows() &&
                windowsJob is { IsInvalid: false, IsClosed: false })
            {
                return WindowsNativeMethods.HasActiveProcesses(windowsJob);
            }

            return (process is not null && IsRunning(process)) ||
                   descendants.Any(IsRunning);
        }

        public void Kill()
        {
            Process? process;
            SafeFileHandle? windowsJob;
            Process[] descendants;
            Timer? descendantCaptureTimer;

            lock (_lock)
            {
                _killRequested = true;
                process = _process;
                windowsJob = _windowsJob;
                descendants = [.. _descendants.Values];
                descendantCaptureTimer = _descendantCaptureTimer;
                _descendantCaptureTimer = null;
            }

            descendantCaptureTimer?.Dispose();

            if (process is not null)
            {
                TryKill(process, windowsJob);
            }

            foreach (var descendant in descendants)
            {
                TryKill(descendant, null);
            }
        }

        public void Dispose()
        {
            SafeFileHandle? windowsJob;
            Process[] descendants;
            Timer? descendantCaptureTimer;

            lock (_lock)
            {
                _disposed = true;
                windowsJob = _windowsJob;
                descendants = [.. _descendants.Values];
                descendantCaptureTimer = _descendantCaptureTimer;
                _windowsJob = null;
                _process = null;
                _descendantCaptureTimer = null;
                _descendants.Clear();
            }

            descendantCaptureTimer?.Dispose();
            windowsJob?.Dispose();

            foreach (var descendant in descendants)
            {
                descendant.Dispose();
            }
        }

        private void CaptureDescendants()
        {
            int[] processIds;

            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }

                try
                {
                    processIds = _process is null
                        ? []
                        : [_process.Id, .. _descendants.Keys];
                }
                catch (InvalidOperationException)
                {
                    processIds = [.. _descendants.Keys];
                }
            }

            try
            {
                foreach (var processId in processIds.SelectMany(GetDescendantProcessIds).Distinct())
                {
                    Process? descendant = null;

                    try
                    {
                        descendant = Process.GetProcessById(processId);
                        var capturedDescendant = descendant;
                        var killDescendant = false;

                        lock (_lock)
                        {
                            if (_disposed)
                            {
                                descendant.Dispose();
                            }
                            else if (_descendants.TryAdd(processId, descendant))
                            {
                                killDescendant = _killRequested;
                            }
                            else
                            {
                                descendant.Dispose();
                            }

                            descendant = null;
                        }

                        if (killDescendant)
                        {
                            TryKill(capturedDescendant, null);
                        }
                    }
                    catch (ArgumentException)
                    {
                        // The descendant exited while it was being captured.
                    }
                    finally
                    {
                        descendant?.Dispose();
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // A process exited while its descendants were being captured.
            }
            catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
            {
                // Timer and cancellation callbacks must never propagate process-discovery failures.
            }
        }

        private static bool IsRunning(Process process)
        {
            try
            {
                return !process.HasExited;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            catch (Win32Exception)
            {
                return false;
            }
        }

        private static HashSet<int> GetDescendantProcessIds(int rootProcessId)
        {
            var pendingProcessIds = new Queue<int>();
            var descendantProcessIds = new HashSet<int>();
            pendingProcessIds.Enqueue(rootProcessId);

            while (pendingProcessIds.TryDequeue(out var parentProcessId))
            {
                foreach (var childProcessId in GetChildProcessIds(parentProcessId))
                {
                    if (descendantProcessIds.Add(childProcessId))
                    {
                        pendingProcessIds.Enqueue(childProcessId);
                    }
                }
            }

            return descendantProcessIds;
        }

        private static int[] GetChildProcessIds(int parentProcessId)
        {
            if (OperatingSystem.IsLinux())
            {
                var childrenFile = $"/proc/{parentProcessId}/task/{parentProcessId}/children";

                try
                {
                    var processIds = File.ReadAllText(childrenFile)
                        .Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries);
                    return Array.ConvertAll(processIds, int.Parse);
                }
                catch (IOException)
                {
                    return [];
                }
                catch (UnauthorizedAccessException)
                {
                    return [];
                }
            }

            if (OperatingSystem.IsMacOS())
            {
                return MacNativeMethods.GetChildProcessIds(parentProcessId);
            }

            return [];
        }

        [SupportedOSPlatform("windows")]
        private static SafeFileHandle? TryCreateWindowsJob(Process process)
        {
            var job = WindowsNativeMethods.CreateJobObject(nint.Zero, null);
            if (job.IsInvalid)
            {
                job.Dispose();
                return null;
            }

            try
            {
                if (WindowsNativeMethods.AssignProcessToJobObject(job, process.SafeHandle))
                {
                    return job;
                }
            }
            catch (InvalidOperationException)
            {
                // The process exited before it could be assigned.
            }

            job.Dispose();
            return null;
        }

        private static void TryKill(Process process, SafeFileHandle? windowsJob)
        {
            try
            {
                if (OperatingSystem.IsWindows() &&
                    windowsJob is { IsInvalid: false, IsClosed: false } &&
                    WindowsNativeMethods.TerminateJobObject(windowsJob, 1))
                {
                    return;
                }

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

        private static class WindowsNativeMethods
        {
            private const int JobObjectBasicAccountingInformation = 1;

            public static bool HasActiveProcesses(SafeFileHandle job)
            {
                if (!QueryInformationJobObject(
                        job,
                        JobObjectBasicAccountingInformation,
                        out var information,
                        (uint) Marshal.SizeOf<BasicAccountingInformation>(),
                        out _))
                {
                    return true;
                }

                return information.ActiveProcesses > 0;
            }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern SafeFileHandle CreateJobObject(nint jobAttributes, string? name);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AssignProcessToJobObject(
                SafeFileHandle job,
                SafeProcessHandle process);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool TerminateJobObject(SafeFileHandle job, uint exitCode);

            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool QueryInformationJobObject(
                SafeFileHandle job,
                int informationClass,
                out BasicAccountingInformation information,
                uint informationLength,
                out uint returnLength);
#pragma warning restore SYSLIB1054

            [StructLayout(LayoutKind.Sequential)]
            private struct BasicAccountingInformation
            {
                public long TotalUserTime;
                public long TotalKernelTime;
                public long ThisPeriodTotalUserTime;
                public long ThisPeriodTotalKernelTime;
                public uint TotalPageFaultCount;
                public uint TotalProcesses;
                public uint ActiveProcesses;
                public uint TotalTerminatedProcesses;
            }
        }

        [SupportedOSPlatform("macos")]
        private static class MacNativeMethods
        {
            public static int[] GetChildProcessIds(int parentProcessId)
            {
                var processCount = ProcListChildPids(parentProcessId, null, 0);
                if (processCount <= 0)
                {
                    return [];
                }

                var processIds = new int[processCount];
                var actualProcessCount =
                    ProcListChildPids(parentProcessId, processIds, processIds.Length * sizeof(int));
                return actualProcessCount > 0
                    ? processIds[..Math.Min(actualProcessCount, processIds.Length)]
                    : [];
            }

#pragma warning disable SYSLIB1054 // LibraryImport requires unsafe blocks, which this project does not enable.
            [DllImport("libproc", EntryPoint = "proc_listchildpids")]
            private static extern int ProcListChildPids(
                int parentProcessId,
                int[]? processIds,
                int byteCount);
#pragma warning restore SYSLIB1054
        }
    }

    private static PipeTarget CreateOutputTarget(
        BoundedCommandOutputBuffer buffer,
        BoundedCommandOutputBuffer? completeOutputBuffer,
        Action<string>? logLine)
    {
        var captureTarget = CreateCaptureTarget(buffer, completeOutputBuffer);
        return logLine is null
            ? captureTarget
            : PipeTarget.Merge(captureTarget, PipeTarget.ToDelegate(logLine));
    }

    private void LogCommandCompletion(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string input,
        int exitCode,
        TimeSpan runTime,
        string standardOutput,
        string standardError,
        BoundedCommandOutputBuffer? completeStandardOutputBuffer,
        BoundedCommandOutputBuffer? completeStandardErrorBuffer,
        DeferredCommandOutputLogger? deferredOutputLogger,
        string workingDirectory)
    {
        var deferredOutput = deferredOutputLogger?.Complete();
        var hasStreamedOutput = deferredOutput?.HasStreamedOutput == true;
        _commandLogger.LogCommandCompletion(
            options,
            executionOptions,
            input,
            exitCode,
            runTime,
            hasStreamedOutput
                ? string.Empty
                : deferredOutput?.PendingStandardOutput
                  ?? completeStandardOutputBuffer?.ToString()
                  ?? standardOutput,
            hasStreamedOutput
                ? string.Empty
                : completeStandardErrorBuffer?.ToString() ?? standardError,
            workingDirectory);
    }

    private static PipeTarget CreateCaptureTarget(
        BoundedCommandOutputBuffer buffer,
        BoundedCommandOutputBuffer? completeOutputBuffer)
    {
        return PipeTarget.Create(async (stream, cancellationToken) =>
        {
            using var reader = new StreamReader(
                stream,
                Encoding.Default,
                detectEncodingFromByteOrderMarks: true,
                bufferSize: 4096,
                leaveOpen: true);
            var characters = new char[4096];
            int charactersRead;
            while ((charactersRead = await reader.ReadAsync(characters, cancellationToken).ConfigureAwait(false)) > 0)
            {
                buffer.Append(characters.AsSpan(0, charactersRead));
                completeOutputBuffer?.Append(characters.AsSpan(0, charactersRead));
            }
        });
    }
}
