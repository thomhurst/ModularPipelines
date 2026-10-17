using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using ModularPipelines.Constants;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.Tracing;
using CommandResult = ModularPipelines.CommandResult;

namespace ModularPipelines.Context;

/// <summary>
/// Orchestrates command-line tool execution by coordinating argument building,
/// placeholder replacement, and command execution.
/// </summary>
internal sealed class Command(
    ICommandLogger commandLogger,
    ICommandLineBuilder commandLineBuilder,
    IEnumerable<ICommandInterceptor> commandInterceptors,
    ISecretProvider secretProvider,
    ISecretRegistry secretRegistry,
    ISecretObfuscator secretObfuscator,
    ICommandExecutionCounter commandExecutionCounter,
    IOptions<PipelineOptions> pipelineOptions,
    PipelineWorkingDirectory pipelineWorkingDirectory,
    WindowsJobLifetime windowsJobLifetime) : ICommandContext
{
    // Win32 ERROR_FILE_NOT_FOUND and Unix ENOENT both use native error code 2.
    private const int FileNotFoundNativeErrorCode = 2;

    private sealed record PreparedCommandInvocation(
        CommandLine CommandLine,
        CommandLineToolOptions ToolOptions,
        CommandExecutionOptions ExecutionOptions,
        string RawCommandInput,
        string WorkingDirectory,
        IReadOnlyDictionary<string, string?> RawEnvironmentVariables,
        int SudoPrefixLength,
        ProcessCommand Command,
        Lazy<string> InputToLog);

    private readonly ICommandLogger _commandLogger = commandLogger;
    private readonly WindowsJobLifetime _windowsJobLifetime = windowsJobLifetime;
    private readonly ICommandLineBuilder _commandLineBuilder = commandLineBuilder;
    private readonly IEnumerable<ICommandInterceptor> _commandInterceptors = commandInterceptors;
    private readonly ISecretProvider _secretProvider = secretProvider;
    private readonly ISecretRegistry _secretRegistry = secretRegistry;
    private readonly ISecretObfuscator _secretObfuscator = secretObfuscator;
    private readonly ICommandExecutionCounter _commandExecutionCounter = commandExecutionCounter;
    private readonly IOptions<PipelineOptions> _pipelineOptions = pipelineOptions;
    private readonly PipelineWorkingDirectory _pipelineWorkingDirectory = pipelineWorkingDirectory;

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
        (ProcessCommand Command, string CommandInput, string Tool, List<string> ParsedArgs, int SudoPrefixLength) commandDetails;
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
        var (command, commandInput, tool, parsedArgs, sudoPrefixLength) = commandDetails;

        cancellationToken.ThrowIfCancellationRequested();

        using var timeoutCancellationToken = CreateTimeoutCancellationToken(execOpts);
        using var linkedCancellationToken =
            CreateLinkedCancellationToken(timeoutCancellationToken, cancellationToken);
        var obfuscatedTool = _secretObfuscator.Obfuscate(tool, execOpts);
        using var activity = ModuleActivityTracing.StartCommandActivity(obfuscatedTool);
        var invocation = new PreparedCommandInvocation(
            new CommandLine(tool, parsedArgs),
            options,
            execOpts,
            commandInput,
            command.WorkingDirPath,
            GetRawEnvironmentVariables(command),
            sudoPrefixLength,
            command,
            CreateInputToLog(commandInput, execOpts, activity));

        try
        {
            return await ExecuteCommandCoreAsync(
                    invocation,
                    activity,
                    linkedCancellationToken.Token,
                    cancellationToken,
                    timeoutCancellationToken)
                .ConfigureAwait(false);
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
        if (executionOptions.StandardInputIsSecret)
        {
            _secretRegistry.AddSecret(executionOptions.StandardInput);
        }
    }

    private (ProcessCommand Command, string CommandInput, string Tool, List<string> Arguments, int SudoPrefixLength) CreateCommand(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions)
    {
        var commandLine = _commandLineBuilder.Build(options);
        var tool = commandLine.Tool;
        var arguments = commandLine.Arguments.ToList();
        var sudoPrefixLength = 0;
        if (executionOptions.Sudo)
        {
            var sudoArguments = CreateSudoPrefix(executionOptions);
            sudoPrefixLength = sudoArguments.Count;
            sudoArguments.Add(tool);
            arguments.InsertRange(0, sudoArguments);
            tool = "sudo";
        }

        var (command, input) = CreateCliCommand(tool, arguments, executionOptions);
        return (command, input, tool, arguments, sudoPrefixLength);
    }

    private static List<string> CreateSudoPrefix(CommandExecutionOptions executionOptions)
    {
        var sudoArguments = new List<string> { "-n" };
        var preservedVariables = executionOptions.EnvironmentVariables?
            .Where(pair => pair.Value is not null)
            .Select(pair => pair.Key)
            .ToArray() ?? [];
        if (preservedVariables.Any(name => name.Contains(',')))
        {
            throw new ArgumentException(
                "Environment variable names cannot contain commas when Sudo is enabled.",
                nameof(executionOptions));
        }

        if (preservedVariables.Length > 0)
        {
            sudoArguments.Add($"--preserve-env={string.Join(',', preservedVariables)}");
        }

        sudoArguments.Add("--");
        return sudoArguments;
    }

    private static (ProcessCommand Command, string Input) CreateCliCommand(
        string tool,
        IEnumerable<string> arguments,
        CommandExecutionOptions executionOptions)
    {
        var preparedCommand = CliCommandFactory.Create(tool, arguments, executionOptions);
        return (preparedCommand.Command, preparedCommand.Input);
    }

    private Lazy<string> CreateInputToLog(
        string commandInput,
        CommandExecutionOptions executionOptions,
        Activity? activity)
    {
        return new Lazy<string>(() =>
        {
            var input = GetInputToLog(commandInput, executionOptions);
            RecordTelemetryCommandInput(activity, input, executionOptions);
            return input;
        });
    }

    /// <summary>
    /// Prepares a command that an interceptor changed before passing it on. The command line is used
    /// as given, except that a retained framework-generated sudo prefix is refreshed for environment changes.
    /// </summary>
    private PreparedCommandInvocation PrepareChangedInvocation(
        CommandInvocation invocation,
        PreparedCommandInvocation received,
        Activity? activity)
    {
        var toolOptions = invocation.ToolOptions ?? received.ToolOptions;
        var workingDirectory = string.Equals(
            invocation.WorkingDirectory,
            received.WorkingDirectory,
            StringComparison.Ordinal)
            ? invocation.ExecutionOptions.WorkingDirectory
            : invocation.WorkingDirectory;
        var executionOptions = invocation.ExecutionOptions with
        {
            WorkingDirectory = workingDirectory is null
                ? _pipelineWorkingDirectory.Path
                : _pipelineWorkingDirectory.ResolvePath(workingDirectory),
        };
        RegisterSecrets(toolOptions, executionOptions);
        var commandLine = invocation.CommandLine;
        var sudoPrefixLength = 0;
        if (received.SudoPrefixLength > 0
            && string.Equals(commandLine.Tool, received.CommandLine.Tool, StringComparison.Ordinal)
            && commandLine.Arguments.Take(received.SudoPrefixLength)
                .SequenceEqual(received.CommandLine.Arguments.Take(received.SudoPrefixLength), StringComparer.Ordinal))
        {
            var sudoArguments = CreateSudoPrefix(executionOptions);
            sudoPrefixLength = sudoArguments.Count;
            sudoArguments.AddRange(commandLine.Arguments.Skip(received.SudoPrefixLength));
            commandLine = new CommandLine(commandLine.Tool, sudoArguments);
        }

        var (command, commandInput) = CreateCliCommand(
            commandLine.Tool,
            commandLine.Arguments,
            executionOptions);
        return new PreparedCommandInvocation(
            commandLine,
            toolOptions,
            executionOptions,
            commandInput,
            command.WorkingDirPath,
            GetRawEnvironmentVariables(command),
            sudoPrefixLength,
            command,
            CreateInputToLog(commandInput, executionOptions, activity));
    }

    private async Task<CommandResult> ExecuteCommandCoreAsync(
        PreparedCommandInvocation invocation,
        Activity? activity,
        CancellationToken executionCancellationToken,
        CancellationToken callerCancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        executionCancellationToken.ThrowIfCancellationRequested();

        var interceptors = _commandInterceptors as ICommandInterceptor[] ?? [.. _commandInterceptors];
        if (interceptors.Length == 0)
        {
            var result = await ExecuteInvocationAsync(
                    invocation,
                    executionCancellationToken,
                    callerCancellationToken,
                    timeoutCancellationToken)
                .ConfigureAwait(false);
            ModuleActivityTracing.RecordCommandResult(activity, result, invocation.ExecutionOptions.SuccessExitCodes.Contains(result.ExitCode));
            return result;
        }

        var pipeline = new InterceptionPipeline(this, invocation, activity, callerCancellationToken, timeoutCancellationToken);
        var interceptedResult = await pipeline.ExecuteAsync(interceptors, executionCancellationToken).ConfigureAwait(false);
        ModuleActivityTracing.RecordCommandResult(activity, interceptedResult, pipeline.ExecutionOptions.SuccessExitCodes.Contains(interceptedResult.ExitCode));
        return interceptedResult;
    }

    private async ValueTask<CommandResult> ExecuteInvocationAsync(
        PreparedCommandInvocation invocation,
        CancellationToken executionCancellationToken,
        CancellationToken callerCancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        executionCancellationToken.ThrowIfCancellationRequested();

        return invocation.ExecutionOptions.InternalDryRun
            ? ExecuteDryRun(
                invocation.Command,
                invocation.RawCommandInput,
                invocation.ToolOptions,
                invocation.ExecutionOptions,
                invocation.InputToLog,
                invocation.RawEnvironmentVariables)
            : await Of(
                    invocation.Command,
                    invocation.RawCommandInput,
                    invocation.ToolOptions,
                    invocation.ExecutionOptions,
                    invocation.InputToLog,
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

    /// <summary>
    /// Completes a result returned by an interceptor that did not call the rest of the pipeline.
    /// </summary>
    private CommandResult CompleteShortCircuitedResult(
        CommandResult intercepted,
        PreparedCommandInvocation invocation)
    {
        var executionOptions = invocation.ExecutionOptions;
        var result = ApplyCommandMetadata(
            intercepted,
            invocation.Command,
            invocation,
            executionOptions);
        LogInterceptedCommand(invocation.ToolOptions, executionOptions, invocation.InputToLog.Value, result);
        if (executionOptions.ShouldThrowForExitCode(result.ExitCode))
        {
            throw CommandException.FromAlreadyObfuscatedResult(CreateFailureResult(
                invocation.Command,
                executionOptions,
                result.CommandInput,
                result.ExitCode,
                result.Duration,
                result.StandardOutput,
                result.StandardError,
                invocation.RawEnvironmentVariables,
                result.StartTime,
                result.EndTime,
                result.StandardOutputTruncatedCharacters,
                result.StandardErrorTruncatedCharacters));
        }

        return result;
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

    /// <summary>
    /// Runs the registered interceptors as middleware around process execution. The first registered
    /// interceptor is outermost.
    /// </summary>
    private sealed class InterceptionPipeline(
        Command owner,
        PreparedCommandInvocation original,
        Activity? activity,
        CancellationToken callerCancellationToken,
        CancellationTokenSource? timeoutCancellationToken)
    {
        private CommandInvocation? _publicInvocation;
        private PreparedCommandInvocation _publicPrepared = original;
        private long _publicSecretVersion = long.MinValue;
        private CancellationToken _executionCancellationToken;

        public CommandExecutionOptions ExecutionOptions { get; private set; } = original.ExecutionOptions;

        public ValueTask<CommandResult> ExecuteAsync(
            IReadOnlyList<ICommandInterceptor> interceptors,
            CancellationToken cancellationToken)
        {
            _executionCancellationToken = cancellationToken;
            CommandDelegate pipeline = TerminalAsync;
            for (var index = interceptors.Count - 1; index >= 0; index--)
            {
                var interceptor = interceptors[index];
                var next = pipeline;
                pipeline = (invocation, token) => InvokeInterceptorAsync(interceptor, invocation, next, token);
            }

            return pipeline(Publish(_publicPrepared), cancellationToken);
        }

        private async ValueTask<CommandResult> InvokeInterceptorAsync(
            ICommandInterceptor interceptor,
            CommandInvocation invocation,
            CommandDelegate next,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var prepared = Resolve(invocation);
            var publicInvocation = ReferenceEquals(invocation, _publicInvocation)
                                   && _publicSecretVersion == owner._secretProvider.Version
                ? invocation
                : Publish(prepared);

            var nextCalled = false;
            var result = await interceptor
                             .InvokeAsync(
                                 publicInvocation,
                                 (nextInvocation, nextToken) =>
                                 {
                                     nextCalled = true;
                                     return next(nextInvocation, nextToken);
                                 },
                                 cancellationToken)
                             .ConfigureAwait(false)
                         ?? throw new InvalidOperationException(
                             $"Command interceptor '{interceptor.GetType().FullName}' returned a null result.");
            cancellationToken.ThrowIfCancellationRequested();

            if (nextCalled)
            {
                return result;
            }

            ExecutionOptions = prepared.ExecutionOptions;
            return owner.CompleteShortCircuitedResult(result, prepared);
        }

        private async ValueTask<CommandResult> TerminalAsync(
            CommandInvocation invocation,
            CancellationToken cancellationToken)
        {
            var prepared = Resolve(invocation);
            ExecutionOptions = prepared.ExecutionOptions;
            // An interceptor may pass its own token, but it cannot drop the caller and timeout cancellation.
            if (!cancellationToken.CanBeCanceled || cancellationToken == _executionCancellationToken)
            {
                return await owner.ExecuteInvocationAsync(
                        prepared,
                        _executionCancellationToken,
                        callerCancellationToken,
                        timeoutCancellationToken)
                    .ConfigureAwait(false);
            }

            using var linked = CancellationTokenSource.CreateLinkedTokenSource(
                _executionCancellationToken,
                cancellationToken);
            return await owner.ExecuteInvocationAsync(
                    prepared,
                    linked.Token,
                    callerCancellationToken,
                    timeoutCancellationToken)
                .ConfigureAwait(false);
        }

        private PreparedCommandInvocation Resolve(CommandInvocation invocation)
        {
            if (ReferenceEquals(invocation, _publicInvocation))
            {
                return _publicPrepared;
            }

            var received = _publicPrepared;
            if (Equals(invocation.CommandLine, received.CommandLine)
                && Equals(invocation.ExecutionOptions, received.ExecutionOptions)
                && string.Equals(invocation.WorkingDirectory, received.WorkingDirectory, StringComparison.Ordinal))
            {
                return invocation.ToolOptions is null || ReferenceEquals(invocation.ToolOptions, received.ToolOptions)
                    ? received
                    : received with { ToolOptions = invocation.ToolOptions };
            }

            return owner.PrepareChangedInvocation(invocation, received, activity);
        }

        private CommandInvocation Publish(PreparedCommandInvocation prepared)
        {
            var (invocation, secretVersion) = owner.CreatePublicInvocation(prepared, prepared.ExecutionOptions);
            _publicInvocation = invocation;
            _publicPrepared = prepared;
            _publicSecretVersion = secretVersion;
            return invocation;
        }
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
        ProcessCommand command,
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
            command.WorkingDirPath,
            _secretObfuscator.Obfuscate(commandInput, executionOptions),
            ObfuscateEnvironmentVariables(rawEnvironmentVariables, executionOptions));
    }

    private CommandResult ApplyCommandMetadata(
        CommandResult result,
        ProcessCommand command,
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
        ProcessCommand command,
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
        using var processTreeTerminator = new ProcessTreeTerminator(_commandLogger, _windowsJobLifetime);
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
            ProcessRunResult result;
            try
            {
                var executionTask = ProcessRunner.RunAsync(
                    command,
                    new CommandOutputTarget(
                        standardOutputBuffer,
                        completeStandardOutputBuffer,
                        CreateStandardOutputLogger(deferredOutputLogger)),
                    new CommandOutputTarget(
                        standardErrorBuffer,
                        completeStandardErrorBuffer,
                        CreateStandardErrorLogger(deferredOutputLogger)),
                    execOpts.OutputEncoding,
                    execOpts.OutputDrainTimeout,
                    processTreeTerminator.Attach,
                    executionCancellationToken,
                    execOpts.StandardInput);
                using var descendantCaptureRegistration =
                    executionCancellationToken.Register(processTreeTerminator.BeginGracefulShutdown);
                result = await executionTask.ConfigureAwait(false);

                await WaitForForcefulCancellationAsync(
                    processTreeTerminator,
                    executionCancellationToken,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = GetDiagnosticOutput(standardOutputBuffer, execOpts);
                standardError = GetDiagnosticOutput(standardErrorBuffer, execOpts);
            }
            catch (Exception e) when (e is not CommandException)
            {
                await WaitForForcefulCancellationAsync(
                    processTreeTerminator,
                    executionCancellationToken,
                    forcefulCancellationToken.Token).ConfigureAwait(false);

                standardOutput = GetDiagnosticOutput(standardOutputBuffer, execOpts);
                standardError = GetDiagnosticOutput(standardErrorBuffer, execOpts);

                loggingFailures.Capture(
                    () => LogCommandCompletion(
                        options,
                        execOpts,
                        inputToLog,
                        null,
                        stopwatch.Elapsed,
                        standardOutput,
                        standardError,
                        completeStandardOutputBuffer,
                        completeStandardErrorBuffer,
                        deferredOutputLogger,
                        command.WorkingDirPath,
                        standardOutputBuffer.TruncatedCharacters,
                        standardErrorBuffer.TruncatedCharacters));
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
                    timeoutCancellationToken,
                    standardOutputBuffer.TruncatedCharacters,
                    standardErrorBuffer.TruncatedCharacters);
            }

            if (result.OutputMayBeIncomplete)
            {
                loggingFailures.Capture(() => _commandLogger.LogOutputDrainTimeout(options, execOpts));
            }

            var commandFailure = CreateCommandFailure(
                command,
                result,
                execOpts,
                commandInput,
                standardOutput,
                standardError,
                rawEnvironmentVariables,
                standardOutputBuffer.TruncatedCharacters,
                standardErrorBuffer.TruncatedCharacters);

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
                    command.WorkingDirPath,
                    standardOutputBuffer.TruncatedCharacters,
                    standardErrorBuffer.TruncatedCharacters));
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
                commandInput: _secretObfuscator.Obfuscate(commandInput, execOpts),
                workingDirectory: command.WorkingDirPath,
                standardOutput: standardOutputBuffer.TruncatedCharacters > 0 ? standardOutputBuffer.ToString() : standardOutput,
                standardError: standardErrorBuffer.TruncatedCharacters > 0 ? standardErrorBuffer.ToString() : standardError,
                environmentVariables: ObfuscateEnvironmentVariables(rawEnvironmentVariables, execOpts),
                startTime: result.StartTime,
                endTime: result.ExitTime,
                duration: result.RunTime,
                exitCode: result.ExitCode)
            {
                StandardOutputTruncatedCharacters = standardOutputBuffer.TruncatedCharacters,
                StandardErrorTruncatedCharacters = standardErrorBuffer.TruncatedCharacters,
                OutputMayBeIncomplete = result.OutputMayBeIncomplete,
            };
        }
    }

    private string GetDiagnosticOutput(BoundedCommandOutputBuffer buffer, CommandExecutionOptions options)
    {
        if (buffer.TruncatedCharacters == 0)
        {
            return buffer.ToString();
        }

        return buffer.ToString((value, truncatedStart, truncatedEnd) =>
            _secretObfuscator.ObfuscateTruncated(value, options, truncatedStart, truncatedEnd));
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
        ProcessCommand command,
        CommandExecutionOptions execOpts,
        string input,
        TimeSpan duration,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        CancellationToken cancellationToken,
        CancellationTokenSource? timeoutCancellationToken,
        long standardOutputTruncatedCharacters,
        long standardErrorTruncatedCharacters)
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
            rawEnvironmentVariables,
            standardOutputTruncatedCharacters: standardOutputTruncatedCharacters,
            standardErrorTruncatedCharacters: standardErrorTruncatedCharacters);
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

    internal static TimeoutException CreateTimeoutException(
        CommandExecutionOptions executionOptions,
        Exception innerException)
    {
        var timeout = executionOptions.ExecutionTimeout!.Value;
        var source = timeout == CommandExecutionOptions.DefaultExecutionTimeout
            ? "the default CommandExecutionOptions.ExecutionTimeout"
            : "CommandExecutionOptions.ExecutionTimeout";
        return new TimeoutException(
            $"Command execution timed out after {timeout} ({source}). "
            + "Raise CommandExecutionOptions.ExecutionTimeout for long-running commands, "
            + "or set it to null to disable the timeout.",
            innerException);
    }

    private CommandResult CreateFailureResult(
        ProcessCommand command,
        CommandExecutionOptions execOpts,
        string input,
        int exitCode,
        TimeSpan duration,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        DateTimeOffset? startTime = null,
        DateTimeOffset? endTime = null,
        long standardOutputTruncatedCharacters = 0,
        long standardErrorTruncatedCharacters = 0)
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
            exitCode: exitCode)
        {
            StandardOutputTruncatedCharacters = standardOutputTruncatedCharacters,
            StandardErrorTruncatedCharacters = standardErrorTruncatedCharacters,
        };
    }

    private static Dictionary<string, string?> GetRawEnvironmentVariables(ProcessCommand command)
    {
        var variables = new Dictionary<string, string?>(OperatingSystem.IsWindows()
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal);

        foreach (var pair in command.EnvironmentVariables)
        {
            if (!CliCommandFactory.IsInternalEnvironmentVariable(pair.Key))
            {
                // Match ProcessCommand.CreateStartInfo: the last value for a name wins.
                variables[pair.Key] = pair.Value;
            }
        }

        return variables;
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
        ProcessCommand command,
        ProcessRunResult result,
        CommandExecutionOptions execOpts,
        string input,
        string standardOutput,
        string standardError,
        IReadOnlyDictionary<string, string?> rawEnvironmentVariables,
        long standardOutputTruncatedCharacters,
        long standardErrorTruncatedCharacters)
    {
        return execOpts.ShouldThrowForExitCode(result.ExitCode)
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
                result.ExitTime,
                standardOutputTruncatedCharacters,
                standardErrorTruncatedCharacters) with
            { OutputMayBeIncomplete = result.OutputMayBeIncomplete })
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
        return !loggingOptions.IncludesCommandArguments
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
                // Windows commands cannot receive a graceful interrupt, so waiting cannot help them exit.
                forcefulCancellationToken.CancelAfter(OperatingSystem.IsWindows() ? TimeSpan.Zero : gracefulShutdownTimeout);
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

    private sealed class ProcessTreeTerminator(ICommandLogger commandLogger, WindowsJobLifetime windowsJobLifetime) : IDisposable
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
            if (windowsJob is not null)
            {
                windowsJobLifetime.Add(windowsJob);
            }

            lock (_lock)
            {
                _process = process;
                _windowsJob = windowsJob;

                if (!_killRequested)
                {
                    return;
                }
            }

            TryKill(process, windowsJob, ProcessTerminationReason.StartedAfterCancellation);
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
                TryKill(process, windowsJob, ProcessTerminationReason.ForcefulCancellation);
            }

            foreach (var descendant in descendants)
            {
                TryKill(descendant, null, ProcessTerminationReason.ForcefulCancellation);
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
            // A completed command may have left a server running for another module. Keep that job
            // alive until pipeline disposal; empty jobs can release their native handles now.
            if (OperatingSystem.IsWindows() && windowsJob is { IsClosed: false } &&
                !WindowsNativeMethods.HasActiveProcesses(windowsJob))
            {
                windowsJobLifetime.Release(windowsJob);
            }

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
                foreach (var processId in processIds.SelectMany(ProcessTree.GetDescendantProcessIds).Distinct())
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
                            TryKill(capturedDescendant, null, ProcessTerminationReason.DescendantCapturedAfterCancellation);
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
                if (WindowsNativeMethods.EnableKillOnClose(job) &&
                    WindowsNativeMethods.AssignProcessToJobObject(job, process.SafeHandle))
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

        private void TryKill(Process process, SafeFileHandle? windowsJob, ProcessTerminationReason reason)
        {
            var target = DescribeProcess(process);
            try
            {
                if (OperatingSystem.IsWindows() &&
                    windowsJob is { IsInvalid: false, IsClosed: false })
                {
                    LogTermination(target, reason, ProcessTerminationMethod.WindowsJob);
                    if (WindowsNativeMethods.TerminateJobObject(windowsJob, 1))
                    {
                        return;
                    }
                }

                LogTermination(target, reason, ProcessTerminationMethod.ProcessTree);
                ProcessTree.Kill(
                    process,
                    descendant => LogTermination(DescribeProcess(descendant), reason, ProcessTerminationMethod.SingleProcess),
                    (descendant, exception) => LogTerminationFailure(DescribeProcess(descendant), exception));
            }
            catch (InvalidOperationException)
            {
                // The process already exited.
            }
            catch (Win32Exception exception)
            {
                // The process already exited or could not be terminated.
                LogTerminationFailure(target, exception);
            }
            catch (NotSupportedException)
            {
                // The process is remote.
            }
            catch (AggregateException exception) when (exception.Flatten().InnerExceptions.All(IsBestEffortKillFailure))
            {
                // .NET 11 reports descendants that exit during the tree walk as an aggregate. This runs from
                // cancellation callbacks, so an escaping exception would crash the process.
            }
        }

        private static bool IsBestEffortKillFailure(Exception exception) =>
            exception is InvalidOperationException or Win32Exception or NotSupportedException;

        // Termination runs from cancellation callbacks: a failing logger must never stop the kill.
        private void LogTermination(
            ProcessTerminationTarget target,
            ProcessTerminationReason reason,
            ProcessTerminationMethod method)
        {
            try
            {
                commandLogger.LogProcessTermination(target, reason, method);
            }
            catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
            {
                Trace.TraceError("Process termination logging failed: {0}", exception);
            }
        }

        private void LogTerminationFailure(ProcessTerminationTarget target, Exception terminationException)
        {
            try
            {
                commandLogger.LogProcessTerminationFailure(target, terminationException);
            }
            catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
            {
                Trace.TraceError("Process termination logging failed: {0}", exception);
            }
        }

        private static ProcessTerminationTarget DescribeProcess(Process process)
        {
            int? processId = null;
            string? processName = null;
            try
            {
                processId = process.Id;
                processName = process.ProcessName;
            }
            catch (Exception exception) when (exception is InvalidOperationException or Win32Exception or NotSupportedException)
            {
                // The process exited or is inaccessible; log whatever was read.
            }

            return new ProcessTerminationTarget(processId, processName);
        }

        private static class WindowsNativeMethods
        {
            private const int JobObjectBasicAccountingInformation = 1;
            private const int JobObjectExtendedLimitInformation = 9;
            private const uint JobObjectLimitKillOnJobClose = 0x00002000;

            public static bool EnableKillOnClose(SafeFileHandle job)
            {
                var information = new ExtendedLimitInformation
                {
                    BasicLimitInformation = new BasicLimitInformation
                    {
                        LimitFlags = JobObjectLimitKillOnJobClose,
                    },
                };
                return SetInformationJobObject(
                    job,
                    JobObjectExtendedLimitInformation,
                    in information,
                    (uint) Marshal.SizeOf<ExtendedLimitInformation>());
            }

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
            private static extern bool SetInformationJobObject(
                SafeFileHandle job,
                int informationClass,
                in ExtendedLimitInformation information,
                uint informationLength);

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
            private struct BasicLimitInformation
            {
                public long PerProcessUserTimeLimit;
                public long PerJobUserTimeLimit;
                public uint LimitFlags;
                public nuint MinimumWorkingSetSize;
                public nuint MaximumWorkingSetSize;
                public uint ActiveProcessLimit;
                public nuint Affinity;
                public uint PriorityClass;
                public uint SchedulingClass;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct IoCounters
            {
                public ulong ReadOperationCount;
                public ulong WriteOperationCount;
                public ulong OtherOperationCount;
                public ulong ReadTransferCount;
                public ulong WriteTransferCount;
                public ulong OtherTransferCount;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct ExtendedLimitInformation
            {
                public BasicLimitInformation BasicLimitInformation;
                public IoCounters IoInfo;
                public nuint ProcessMemoryLimit;
                public nuint JobMemoryLimit;
                public nuint PeakProcessMemoryUsed;
                public nuint PeakJobMemoryUsed;
            }

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
    }

    private void LogCommandCompletion(
        CommandLineToolOptions options,
        CommandExecutionOptions executionOptions,
        string input,
        int? exitCode,
        TimeSpan runTime,
        string standardOutput,
        string standardError,
        BoundedCommandOutputBuffer? completeStandardOutputBuffer,
        BoundedCommandOutputBuffer? completeStandardErrorBuffer,
        DeferredCommandOutputLogger? deferredOutputLogger,
        string workingDirectory,
        long standardOutputTruncatedCharacters,
        long standardErrorTruncatedCharacters)
    {
        if (standardOutputTruncatedCharacters > 0 || standardErrorTruncatedCharacters > 0)
        {
            _commandLogger.LogOutputTruncation(options, executionOptions, standardOutputTruncatedCharacters, standardErrorTruncatedCharacters,
                executionOptions.MaxCapturedOutputLength);
        }

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
}
