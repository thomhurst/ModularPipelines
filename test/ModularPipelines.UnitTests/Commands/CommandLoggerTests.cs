using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Exceptions;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Commands;

public class CommandLoggerTests : TestBase
{
    [Test]
    public async Task Masks_Secret_Values_From_Command_Options()
    {
        const string secret = "command-option-secret";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        await result.T.ExecuteCommandLineToolAsync(
            new SecretCommandOptions { Secret = secret },
            new CommandExecutionOptions { ThrowOnNonZeroExitCode = false });
        await result.Pipeline.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).DoesNotContain(secret);
        await Assert.That(logFile).Contains("********");
    }

    [Test]
    public async Task Masks_Secret_Values_From_Dry_Run_Command()
    {
        const string secret = "dry-run-command-secret";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        await result.T.ExecuteCommandLineToolAsync(
            new SecretCommandOptions { Secret = secret },
            new CommandExecutionOptions
            {
                InternalDryRun = true,
                LogSettings = new CommandLoggingOptions
                {
                    ShowCommandArguments = true,
                },
            });
        await result.Pipeline.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains("[DRY-RUN]");
        await Assert.That(logFile).DoesNotContain(secret);
        await Assert.That(logFile).Contains("********");
    }

    [Test]
    public async Task OutputLoggingManipulator_Does_Not_Change_Command_Result()
    {
        const string rawOutput = "raw-output";
        const string displayedOutput = "displayed-output";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        var commandResult = await result.T.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions($"Write-Output '{rawOutput}'"),
            new CommandExecutionOptions
            {
                LogSettings = new CommandLoggingOptions
                {
                    ShowCommandArguments = false,
                },
                OutputLoggingManipulator = _ => displayedOutput,
            });
        await result.Pipeline.DisposeAsync();

        await Assert.That(commandResult.StandardOutput).Contains(rawOutput);
        await Assert.That(commandResult.StandardOutput).DoesNotContain(displayedOutput);

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).Contains(displayedOutput);
        await Assert.That(logFile).DoesNotContain(rawOutput);
    }

    [Test]
    public async Task OutputLoggingManipulator_Receives_Complete_Output_When_Result_Is_Truncated()
    {
        const string firstLine = "first-output-line";
        const string secondLine = "second-output-line";
        var receivedOutputs = new List<string>();
        var command = await GetService<ICommandContext>();

        var result = await command.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions($"Write-Output '{firstLine}'; Write-Output '{secondLine}'"),
            new CommandExecutionOptions
            {
                MaxCapturedOutputLength = 10,
                OutputLoggingManipulator = output =>
                {
                    receivedOutputs.Add(output);
                    return output;
                },
            });

        await Assert.That(result.StandardOutput).Contains("truncated");
        await Assert.That(receivedOutputs).Contains(output =>
            output.Contains(firstLine, StringComparison.Ordinal)
            && output.Contains(secondLine, StringComparison.Ordinal)
            && !output.Contains("truncated", StringComparison.Ordinal));
    }

    [Test]
    [MatrixDataSource]
    public async Task Logs_As_Expected_With_Options(
        [Matrix(true, false)] bool logInput,
        [Matrix(true, false)] bool logOutput,
        [Matrix(true, false)] bool logError,
        [Matrix(true, false)] bool logExitCode,
        [Matrix(true, false)] bool logDuration)
    {
        var file = await RunPowershellCommand("""
                                        echo Hello world!
                                        throw "Error!"
                                        """, logInput, logOutput, logError, logExitCode, logDuration);

        var logFile = await File.ReadAllTextAsync(file);

        if (!logInput && !logOutput && !logError && !logDuration && !logExitCode)
        {
            await Assert.That(logFile).DoesNotContain("INFO	[ModularPipelines.Pipeline]");
            return;
        }

        await Assert.That(logFile).Contains("INFO	[ModularPipelines.Pipeline]");

        // New compact format: command is shown inline with working directory
        if (logInput)
        {
            await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}> pwsh -Command \"echo Hello world!");
        }
        else
        {
            await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}> ********");
        }

        // New compact format: output is shown inline with → for short output, or ↳ for multi-line
        if (logOutput)
        {
            // Output can be inline (→) or on separate line (↳)
            var hasInlineOutput = logFile.Contains("→") || logFile.Contains("↳");
            await Assert.That(hasInlineOutput).IsTrue();
        }

        // New compact format: error is shown with ✗ prefix
        if (logError)
        {
            await Assert.That(logFile).Contains("✗");
        }

        // New compact format: duration is shown inline in brackets
        if (logDuration)
        {
            // Duration is now shown inline like [1ms] or [2s]
            var hasDuration = Regex.IsMatch(logFile, @"\[\d+m?s");
            await Assert.That(hasDuration).IsTrue();
        }

        // New compact format: exit code is shown inline
        if (logExitCode)
        {
            await Assert.That(logFile).Contains("exit ");
        }
    }

    private async Task<string> RunPowershellCommand(string command, bool logInput, bool logOutput, bool logError,
        bool logExitCode, bool logDuration)
    {
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => { builder.AddFile(file); });
        });

        // Determine verbosity level based on what's being logged
        var verbosity = (!logInput && !logOutput && !logError && !logExitCode && !logDuration)
            ? CommandLogVerbosity.Silent
            : CommandLogVerbosity.Normal;

        var loggingOptions = new CommandLoggingOptions
        {
            Verbosity = verbosity,
            ShowCommandArguments = logInput,
            ShowStandardOutput = logOutput,
            ShowStandardError = logError,
            ShowExitCode = logExitCode,
            ShowExecutionTime = logDuration,
        };

        await result.T.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions(command),
            new CommandExecutionOptions
            {
                LogSettings = loggingOptions,
                ThrowOnNonZeroExitCode = false,
            });

        await result.Pipeline.DisposeAsync();

        return file;
    }

    [Test]
    public async Task Silent_Verbosity_Logs_Nothing()
    {
        var file = await RunPowershellCommandWithLoggingOptions(
            "echo Hello",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Silent });

        var logFile = await File.ReadAllTextAsync(file);
        // Silent verbosity should not log any command-related output
        // Check for absence of command execution patterns (other pipeline logs may still appear)
        await Assert.That(logFile).DoesNotContain($"{Environment.CurrentDirectory}>");
        await Assert.That(logFile).DoesNotContain("→");
        await Assert.That(logFile).DoesNotContain("↳");
        await Assert.That(logFile).DoesNotContain("exit ");
        await Assert.That(logFile).DoesNotContain("Working Directory:");
    }

    [Test]
    public async Task Minimal_Verbosity_Logs_Only_Input()
    {
        var file = await RunPowershellCommandWithLoggingOptions(
            "echo Hello",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Minimal });

        var logFile = await File.ReadAllTextAsync(file);
        // New compact format: command line includes working directory and command
        await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}>");
        // Minimal doesn't show output, exit code, or duration
        await Assert.That(logFile).DoesNotContain("→");
        await Assert.That(logFile).DoesNotContain("↳");
        await Assert.That(logFile).DoesNotContain("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsFalse();
    }

    [Test]
    public async Task Normal_Verbosity_Logs_Input_And_Output()
    {
        var file = await RunPowershellCommandWithLoggingOptions(
            "echo Hello",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal });

        var logFile = await File.ReadAllTextAsync(file);
        // New compact format: command line includes working directory and command
        await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}>");
        // Fast commands inline output; slower commands may begin streaming under load.
        await Assert.That(Regex.Matches(logFile, "(?:→|↳) Hello").Count).IsEqualTo(1);
        // Normal doesn't show exit code or duration
        await Assert.That(logFile).DoesNotContain("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsFalse();
    }

    [Test]
    public async Task Detailed_Verbosity_Logs_Input_Output_ExitCode_Duration()
    {
        var file = await RunPowershellCommandWithLoggingOptions(
            "echo Hello",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Detailed });

        var logFile = await File.ReadAllTextAsync(file);
        // New compact format: all info on one line
        await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}>");
        // Output is logged exactly once, inline or streamed if startup exceeds the deferral.
        await Assert.That(Regex.Matches(logFile, "(?:→|↳) Hello").Count).IsEqualTo(1);
        // Exit code and duration shown inline
        await Assert.That(logFile).Contains("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue();
    }

    [Test]
    public async Task Diagnostic_Verbosity_Logs_Everything_Including_WorkingDirectory()
    {
        var file = await RunPowershellCommandWithLoggingOptions(
            "echo Hello",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Diagnostic });

        var logFile = await File.ReadAllTextAsync(file);
        // New compact format: all info on one line
        await Assert.That(logFile).Contains($"{Environment.CurrentDirectory}>");
        // Output is logged exactly once, inline or streamed if startup exceeds the deferral.
        await Assert.That(Regex.Matches(logFile, "(?:→|↳) Hello").Count).IsEqualTo(1);
        // Exit code and duration shown inline
        await Assert.That(logFile).Contains("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue();
        // The working directory stays in the command summary instead of adding another log entry.
        await Assert.That(logFile).DoesNotContain("Working Directory:");
    }

    [Test]
    public async Task Command_Logs_Complete_Output_When_Result_Is_Truncated()
    {
        const string output = "complete-output";
        var file = await RunPowershellCommandWithLoggingOptions(
            $"Write-Output '{output}'",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Normal },
            maxCapturedOutputLength: 4);

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(Regex.Matches(logFile, $"(?:→|↳) {output}").Count).IsEqualTo(1);
        await Assert.That(logFile).DoesNotContain("truncated");
    }

    [Test]
    public async Task Failed_Fast_Command_Logs_Short_Standard_Output()
    {
        const string output = "failure-output";
        var file = await RunPowershellCommandWithLoggingOptions(
            $"Write-Output '{output}'; exit 1",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Detailed });

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(Regex.Matches(logFile, $"↳ {output}").Count).IsEqualTo(1);
        await Assert.That(logFile).Contains("exit 1");
    }

    private async Task<string> RunPowershellCommandWithLoggingOptions(
        string command,
        CommandLoggingOptions loggingOptions,
        int? maxCapturedOutputLength = null)
    {
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => { builder.AddFile(file); });
        });

        await result.T.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions(command),
            new CommandExecutionOptions
            {
                LogSettings = loggingOptions,
                ThrowOnNonZeroExitCode = false,
                MaxCapturedOutputLength =
                    maxCapturedOutputLength ?? CommandExecutionOptions.DefaultMaxCapturedOutputLength,
            });

        await result.Pipeline.DisposeAsync();

        return file;
    }

    [Test]
    public async Task Command_Header_Precedes_Streamed_Output_And_Completion()
    {
        var marker = $"ordered-output-{Guid.NewGuid():N}";
        var file = await RunPowershellCommandWithLoggingOptions(
            $"Write-Output '{marker}'; Start-Sleep -Milliseconds 750",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Detailed });

        var logFile = string.Empty;
        var logCompleted = await WaitUntilAsync(
            () =>
            {
                logFile = File.ReadAllText(file);
                return logFile.Contains($"↳ {marker}", StringComparison.Ordinal)
                       && logFile.Contains("✓ [", StringComparison.Ordinal);
            },
            TestHostSettings.DefaultTestTimeout);
        await Assert.That(logCompleted).IsTrue();

        var headerIndex = logFile.IndexOf(
            $"{Environment.CurrentDirectory}> pwsh",
            StringComparison.Ordinal);
        var outputIndex = logFile.IndexOf($"↳ {marker}", StringComparison.Ordinal);
        var completionIndex = logFile.LastIndexOf("✓ [", StringComparison.Ordinal);

        await Assert.That(headerIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(outputIndex).IsGreaterThan(headerIndex);
        await Assert.That(completionIndex).IsGreaterThan(outputIndex);

        var lines = logFile.Split(Environment.NewLine);
        var headerLine = lines.Single(line =>
            line.Contains($"{Environment.CurrentDirectory}> pwsh", StringComparison.Ordinal));
        var completionLine = lines.Last(line =>
            line.Contains("✓ [", StringComparison.Ordinal));
        await Assert.That(headerLine).DoesNotContain("✓");
        await Assert.That(completionLine).Contains("pwsh");
    }

    [Test]
    public async Task Failed_Command_Logs_Error_Before_Completion()
    {
        var marker = $"ordered-error-{Guid.NewGuid():N}";
        var file = await RunPowershellCommandWithLoggingOptions(
            $"[Console]::Error.WriteLine('{marker}'); Start-Sleep -Milliseconds 750; exit 7",
            new CommandLoggingOptions { Verbosity = CommandLogVerbosity.Detailed });

        var logFile = await File.ReadAllTextAsync(file);
        var headerIndex = logFile.IndexOf(
            $"{Environment.CurrentDirectory}> pwsh",
            StringComparison.Ordinal);
        var errorIndex = logFile.IndexOf($"↳ {marker}", StringComparison.Ordinal);
        var completionIndex = logFile.LastIndexOf("✗ [", StringComparison.Ordinal);

        await Assert.That(headerIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(errorIndex).IsGreaterThan(headerIndex);
        await Assert.That(completionIndex).IsGreaterThan(errorIndex);
    }

    [Test]
    public async Task Command_Output_Is_Logged_Before_Command_Completes()
    {
        var marker = $"live-output-{Guid.NewGuid():N}";
        var errorMarker = $"live-error-{Guid.NewGuid():N}";
        var readyFile = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".ready");
        var releaseFile = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".release");
        using var logObserver = new StreamingLogObserver(marker, errorMarker);
        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddProvider(logObserver));
        });
        var script = $$"""
                      Write-Output '{{marker}}'
                      [Console]::Error.WriteLine('{{errorMarker}}')
                      [System.IO.File]::WriteAllText('{{readyFile}}', 'ready')
                      while (-not (Test-Path '{{releaseFile}}')) { Start-Sleep -Milliseconds 10 }
                      """;

        var commandTask = result.T.ExecuteCommandLineToolAsync(new PowershellScriptOptions(script));

        try
        {
            await Assert.That(await WaitUntilAsync(() => File.Exists(readyFile), TimeSpan.FromSeconds(30))).IsTrue();
            await logObserver.OutputObserved.WaitAsync(TimeSpan.FromSeconds(30));
            await Assert.That(commandTask.IsCompleted).IsFalse();
        }
        finally
        {
            await File.WriteAllTextAsync(releaseFile, "release");
            await commandTask;
        }
    }

    [Test]
    public async Task Deferred_Logging_Failure_After_Success_Is_Not_A_Command_Failure()
    {
        var marker = $"throwing-output-{Guid.NewGuid():N}";
        using var loggingProvider = new SelectiveThrowingLoggerProvider($"  ↳ {marker}");
        var (commandContext, _) = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(
                options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddProvider(loggingProvider));
        });

        var exception = await Assert.ThrowsAsync<AggregateException>(() =>
            commandContext.ExecuteCommandLineToolAsync(
                new PowershellScriptOptions(
                    $"Write-Output '{marker}'; "
                    + "Start-Sleep -Milliseconds 750; "
                    + $"Write-Output '{marker}'")));

        await Assert.That(exception!.InnerExceptions).Count().IsEqualTo(1);
        await Assert.That(exception.InnerExceptions[0]).IsTypeOf<InvalidOperationException>();
        await Assert.That(exception.InnerExceptions[0].Message).IsEqualTo("Logging failed.");
        await Assert.That(loggingProvider.ThrowCount).IsEqualTo(1);
    }

    [Test]
    public async Task HeaderLoggingFailureDoesNotPreventCommandExecution()
    {
        var marker = $"header-failure-{Guid.NewGuid():N}";
        var sideEffectFile = Path.Combine(
            TestContext.WorkingDirectory,
            Guid.NewGuid().ToString("N") + ".txt");
        using var loggingProvider =
            new SelectiveThrowingLoggerProvider($"{Environment.CurrentDirectory}> {marker}");
        var (commandContext, _) = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(
                options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddProvider(loggingProvider));
        });

        var exception = await Assert.ThrowsAsync<AggregateException>(() =>
            commandContext.ExecuteCommandLineToolAsync(
                new PowershellScriptOptions(
                    $"[System.IO.File]::WriteAllText('{sideEffectFile}', 'executed')"),
                new CommandExecutionOptions
                {
                    InputLoggingManipulator = _ => marker,
                }));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Flatten().InnerExceptions)
                .Contains(failure =>
                    failure is InvalidOperationException
                    && failure.Message == "Logging failed.");
            await Assert.That(await File.ReadAllTextAsync(sideEffectFile)).IsEqualTo("executed");
            await Assert.That(loggingProvider.ThrowCount).IsEqualTo(1);
        }
    }

    [Test]
    public async Task InvalidExecutionTimeoutDoesNotLogCommandStart()
    {
        var marker = $"invalid-timeout-{Guid.NewGuid():N}";
        var file = Path.Combine(
            TestContext.WorkingDirectory,
            Guid.NewGuid().ToString("N") + ".txt");
        var result = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(
                options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            result.T.ExecuteCommandLineToolAsync(
                new PowershellScriptOptions("Write-Output 'not-executed'"),
                new CommandExecutionOptions
                {
                    ExecutionTimeout = TimeSpan.FromMilliseconds(-2),
                    InputLoggingManipulator = _ => marker,
                }));
        await result.Pipeline.DisposeAsync();

        await Assert.That(await File.ReadAllTextAsync(file)).DoesNotContain(marker);
    }

    [Test]
    public async Task CallerCancellationPreservesCallerTokenIdentity()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        var commandContext = await GetService<ICommandContext>();
        var readyFile = Path.Combine(
            TestContext.WorkingDirectory,
            $"command-cancellation-{Guid.NewGuid():N}.ready");
        var commandTask = commandContext.ExecuteCommandLineToolAsync(
            new PowershellScriptOptions(
                "[IO.File]::WriteAllText($env:MP_COMMAND_CANCELLATION_READY_FILE, 'ready'); "
                + "Start-Sleep -Seconds 30"),
            new CommandExecutionOptions
            {
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["MP_COMMAND_CANCELLATION_READY_FILE"] = readyFile,
                },
                GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
            },
            cancellationToken: cancellationTokenSource.Token);

        try
        {
            await Assert.That(await WaitUntilAsync(
                    () => File.Exists(readyFile),
                    TimeSpan.FromSeconds(30)))
                .IsTrue();
            await cancellationTokenSource.CancelAsync();
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => commandTask);

            await Assert.That(
                    exception!.CancellationToken == cancellationTokenSource.Token)
                .IsTrue();
        }
        finally
        {
            await cancellationTokenSource.CancelAsync();
            try
            {
                await commandTask;
            }
            catch (OperationCanceledException) when (cancellationTokenSource.IsCancellationRequested)
            {
            }

            File.Delete(readyFile);
        }
    }

    [Test]
    public async Task Deferred_Logging_Failure_After_NonZero_Exit_Preserves_Command_Failure()
    {
        var marker = $"throwing-failure-output-{Guid.NewGuid():N}";
        using var loggingProvider = new SelectiveThrowingLoggerProvider($"  ↳ {marker}");
        var (commandContext, _) = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(
                options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddProvider(loggingProvider));
        });

        var exception = await Assert.ThrowsAsync<CommandException>(() =>
            commandContext.ExecuteCommandLineToolAsync(
                new PowershellScriptOptions(
                    $"Write-Output '{marker}'; "
                    + "Start-Sleep -Milliseconds 750; "
                    + "exit 7")));

        await Assert.That(exception!.Result.ExitCode).IsEqualTo(7);
        var failures = (exception.InnerException as AggregateException)
            ?.Flatten().InnerExceptions;
        await Assert.That(failures).IsNotNull();
        await Assert.That(failures!)
            .Contains(failure =>
                failure is CommandException commandFailure
                && commandFailure.Result.ExitCode == 7);
        await Assert.That(failures)
            .Contains(failure =>
                failure is InvalidOperationException
                && failure.Message == "Logging failed.");
        await Assert.That(loggingProvider.ThrowCount).IsEqualTo(1);
    }

    [Test]
    public async Task Deferred_Logging_Failure_During_Cancellation_Is_Wrapped_By_Command()
    {
        var marker = $"throwing-cancellation-output-{Guid.NewGuid():N}";
        using var loggingProvider = new SelectiveThrowingLoggerProvider($"  ↳ {marker}");
        using var cancellationTokenSource = new CancellationTokenSource();
        var (commandContext, _) = await GetService<ICommandContext>(collection =>
        {
            collection.Configure<LoggerFilterOptions>(
                options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddProvider(loggingProvider));
        });

        var commandTask =
            commandContext.ExecuteCommandLineToolAsync(
                new PowershellScriptOptions(
                    $"Write-Output '{marker}'; Start-Sleep -Seconds 30"),
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(100),
                },
                cancellationToken: cancellationTokenSource.Token);

        await loggingProvider.LoggingFailed.WaitAsync(TimeSpan.FromSeconds(30));
        await cancellationTokenSource.CancelAsync();

        var exception = await Assert.ThrowsAsync<CommandException>(() => commandTask);

        var failures = (exception!.InnerException as AggregateException)
            ?.Flatten().InnerExceptions;
        await Assert.That(failures).IsNotNull();
        await Assert.That(failures!)
            .Contains(failure =>
                failure is InvalidOperationException
                && failure.Message == "Logging failed.");
        await Assert.That(loggingProvider.ThrowCount).IsEqualTo(1);
    }

    private static async Task<bool> WaitUntilAsync(Func<bool> condition, TimeSpan timeout)
    {
        var timeoutTask = Task.Delay(timeout);
        while (!condition())
        {
            if (timeoutTask.IsCompleted)
            {
                return false;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(10));
        }

        return true;
    }

    private sealed class StreamingLogObserver(string outputMarker, string errorMarker) : ILoggerProvider
    {
        private readonly Lock _lock = new();
        private readonly TaskCompletionSource _outputObserved =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private bool _sawOutput;
        private bool _sawError;

        public Task OutputObserved => _outputObserved.Task;

        public ILogger CreateLogger(string categoryName)
        {
            return new ObserverLogger(Record);
        }

        public void Dispose()
        {
        }

        private void Record(string message)
        {
            lock (_lock)
            {
                _sawOutput |= message.Contains(outputMarker, StringComparison.Ordinal);
                _sawError |= message.Contains(errorMarker, StringComparison.Ordinal);
                if (_sawOutput && _sawError)
                {
                    _outputObserved.TrySetResult();
                }
            }
        }
    }

    private sealed class ObserverLogger(Action<string> record) : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            record(formatter(state, exception));
        }
    }

    private sealed class SelectiveThrowingLoggerProvider(string messageToThrowOn)
        : ILoggerProvider
    {
        private readonly TaskCompletionSource _loggingFailed =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _throwCount;

        public Task LoggingFailed => _loggingFailed.Task;

        public int ThrowCount => _throwCount;

        public ILogger CreateLogger(string categoryName)
        {
            return new ObserverLogger(message =>
            {
                if (message == messageToThrowOn)
                {
                    Interlocked.Increment(ref _throwCount);
                    _loggingFailed.TrySetResult();
                    throw new InvalidOperationException("Logging failed.");
                }
            });
        }

        public void Dispose()
        {
        }
    }

    [CliTool("pwsh")]
    internal record SecretCommandOptions : CommandLineToolOptions
    {
        [CliOption("-Command")]
        [SecretValue]
        public string? Secret { get; init; }
    }
}
