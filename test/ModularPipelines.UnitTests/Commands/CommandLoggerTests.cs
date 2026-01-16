using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using Vertical.SpectreLogger.Options;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Commands;

public class CommandLoggerTests : TestBase
{
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

        var result = await GetService<ICommand>((_, collection) =>
        {
            collection.Configure<SpectreLoggerOptions>(options => options.MinimumLogLevel = LogLevel.Information);
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
        // Output is shown inline (→ for short output)
        await Assert.That(logFile).Contains("→");
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
        // Output shown inline
        await Assert.That(logFile).Contains("→");
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
        // Output shown inline
        await Assert.That(logFile).Contains("→");
        // Exit code and duration shown inline
        await Assert.That(logFile).Contains("exit ");
        await Assert.That(Regex.IsMatch(logFile, @"\[\d+m?s")).IsTrue();
        // Working directory logged separately at Diagnostic level
        await Assert.That(logFile).Contains("Working Directory:");
    }

    private async Task<string> RunPowershellCommandWithLoggingOptions(string command, CommandLoggingOptions loggingOptions)
    {
        var file = Path.Combine(TestContext.WorkingDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var result = await GetService<ICommand>((_, collection) =>
        {
            collection.Configure<SpectreLoggerOptions>(options => options.MinimumLogLevel = LogLevel.Information);
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
}