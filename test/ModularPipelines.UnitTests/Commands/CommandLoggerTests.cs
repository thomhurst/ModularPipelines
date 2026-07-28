using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
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
        var result = await GetService<ICommandContext>((_, collection) =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        await result.T.ExecuteCommandLineTool(
            new SecretCommandOptions { Secret = secret },
            new CommandExecutionOptions { ThrowOnNonZeroExitCode = false });
        await result.Host.DisposeAsync();

        var logFile = await File.ReadAllTextAsync(file);
        await Assert.That(logFile).DoesNotContain(secret);
        await Assert.That(logFile).Contains("********");
    }

    [Test]
    public async Task OutputLoggingManipulator_Does_Not_Change_Command_Result()
    {
        const string rawOutput = "raw-output";
        const string displayedOutput = "displayed-output";
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");
        var result = await GetService<ICommandContext>((_, collection) =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => builder.AddFile(file));
        });

        var commandResult = await result.T.ExecuteCommandLineTool(
            new PowershellScriptOptions($"Write-Output '{rawOutput}'"),
            new CommandExecutionOptions
            {
                LogSettings = new CommandLoggingOptions
                {
                    ShowCommandArguments = false,
                },
                OutputLoggingManipulator = _ => displayedOutput,
            });
        await result.Host.DisposeAsync();

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
        var command = await GetService<ICommand>();

        var result = await command.ExecuteCommandLineTool(
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

        var result = await GetService<ICommandContext>((_, collection) =>
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

        await result.T.ExecuteCommandLineTool(
            new PowershellScriptOptions(command),
            new CommandExecutionOptions
            {
                LogSettings = loggingOptions,
                ThrowOnNonZeroExitCode = false,
            });

        await result.Host.DisposeAsync();

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
        // Output is streamed on a separate line
        await Assert.That(logFile).Contains("↳");
        await Assert.That(Regex.Matches(logFile, "↳ Hello").Count).IsEqualTo(1);
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
        // Output is streamed on a separate line
        await Assert.That(logFile).Contains("↳");
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
        // Output is streamed on a separate line
        await Assert.That(logFile).Contains("↳");
        // Exit code and duration shown inline
        await Assert.That(logFile).Contains("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue();
        // Working directory logged separately at Diagnostic level
        await Assert.That(logFile).Contains("Working Directory:");
    }

    private async Task<string> RunPowershellCommandWithLoggingOptions(string command, CommandLoggingOptions loggingOptions)
    {
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var result = await GetService<ICommandContext>((_, collection) =>
        {
            collection.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            collection.AddLogging(builder => { builder.AddFile(file); });
        });

        await result.T.ExecuteCommandLineTool(
            new PowershellScriptOptions(command),
            new CommandExecutionOptions
            {
                LogSettings = loggingOptions,
                ThrowOnNonZeroExitCode = false,
            });

        await result.Host.DisposeAsync();

        return file;
    }

    [Test]
    public async Task Command_Output_Is_Logged_Before_Command_Completes()
    {
        var marker = $"live-output-{Guid.NewGuid():N}";
        var errorMarker = $"live-error-{Guid.NewGuid():N}";
        var readyFile = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".ready");
        var releaseFile = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".release");
        using var logObserver = new StreamingLogObserver(marker, errorMarker);
        var result = await GetService<ICommand>((_, collection) =>
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

        var commandTask = result.T.ExecuteCommandLineTool(new PowershellScriptOptions(script));

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

    [CliTool("pwsh")]
    private record SecretCommandOptions : CommandLineToolOptions
    {
        [CliOption("-Command")]
        [SecretValue]
        public string? Secret { get; init; }
    }
}
