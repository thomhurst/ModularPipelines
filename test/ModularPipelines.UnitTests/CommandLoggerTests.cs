using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using Vertical.SpectreLogger.Options;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests;

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
            await Assert.That(logFile).DoesNotContain("INFO	[ModularPipelines.Logging.CommandLogger]");
            return;
        }

        await Assert.That(logFile).Contains("INFO	[ModularPipelines.Logging.CommandLogger]");

        if (logInput)
        {
            await Assert.That(logFile).Contains($"""
                                              ---Executing Command---
                                              {Environment.CurrentDirectory}> pwsh -Command "echo Hello world!
                                              throw \"Error!\""
                                              """);
        }
        else
        {
            await Assert.That(logFile).Contains($"""
                                              ---Executing Command---
                                              {Environment.CurrentDirectory}> ********
                                              """);
        }

        if (logOutput)
        {
            await Assert.That(logFile).Contains("---Command Result---");
        }
        else
        {
            await Assert.That(logFile).DoesNotContain("---Command Result---");
        }

        if (logError)
        {
            await Assert.That(logFile).Contains("---Command Error---");
        }
        else
        {
            await Assert.That(logFile).DoesNotContain("---Command Error---");
        }

        if (logDuration)
        {
            await Assert.That(logFile).Contains("---Duration");
        }
        else
        {
            await Assert.That(logFile).DoesNotContain("---Duration");
        }

        if (logExitCode)
        {
            await Assert.That(logFile).Contains("---Exit Code");
        }
        else
        {
            await Assert.That(logFile).DoesNotContain("---Exit Code");
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

        var logging = CommandLogging.None;

        if (logInput)
        {
            logging |= CommandLogging.Input;
        }

        if (logOutput)
        {
            logging |= CommandLogging.Output;
        }

        if (logError)
        {
            logging |= CommandLogging.Error;
        }

        if (logExitCode)
        {
            logging |= CommandLogging.ExitCode;
        }

        if (logDuration)
        {
            logging |= CommandLogging.Duration;
        }

        await result.T.ExecuteCommandLineTool(new PowershellScriptOptions(command)
        {
            CommandLogging = logging,
            ThrowOnNonZeroExitCode = false,
        });

        await result.Host.DisposeAsync();

        return file;
    }
}