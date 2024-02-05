using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;
using Vertical.SpectreLogger.Options;

namespace ModularPipelines.UnitTests;

public class CommandLoggerTests : TestBase
{
    [Test]
    [Combinatorial]
    public async Task Logs_As_Expected_With_Options(
        [Values(true, false)] bool logInput,
        [Values(true, false)] bool logOutput,
        [Values(true, false)] bool logError,
        [Values(true, false)] bool logExitCode,
        [Values(true, false)] bool logDuration)
    {
        var file = await RunPowershellCommand("""
                                        echo Hello world!
                                        throw "Error!"
                                        """, logInput, logOutput, logError, logExitCode, logDuration);

        var logFile = await File.ReadAllTextAsync(file);

        if (!logInput && !logOutput && !logError && !logDuration && !logExitCode)
        {
            Assert.That(logFile).Does.Not.Contain("INFO	[ModularPipelines.Logging.CommandLogger]");
            return;
        }

        Assert.That(logFile).Does.Contain("INFO	[ModularPipelines.Logging.CommandLogger]");

        if (logInput)
        {
            Assert.That(logFile).Does.Contain("""
                                              ---Executing Command---
                                              pwsh -Command "echo Hello world!
                                              throw \"Error!\""
                                              """);
        }
        else
        {
            Assert.That(logFile).Does.Contain("""
                                              ---Executing Command---
                                              ********
                                              """);
        }

        if (logOutput)
        {
            Assert.That(logFile).Does.Contain("---Command Result---");
        }
        else
        {
            Assert.That(logFile).Does.Not.Contain("---Command Result---");
        }

        if (logError)
        {
            Assert.That(logFile).Does.Contain("---Command Error---");
        }
        else
        {
            Assert.That(logFile).Does.Not.Contain("---Command Error---");
        }
       
        if (logError)
        {
            Assert.That(logFile).Does.Contain("---Duration");
        }
        else
        {
            Assert.That(logFile).Does.Not.Contain("---Duration");
        }
        
        if (logExitCode)
        {
            Assert.That(logFile).Does.Contain("---Exit Code");
        }
        else
        {
            Assert.That(logFile).Does.Not.Contain("---Exit Code");
        }
    }

    private async Task<string> RunPowershellCommand(string command, bool logInput, bool logOutput, bool logError,
        bool logExitCode, bool logDuration)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N") + ".txt");

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