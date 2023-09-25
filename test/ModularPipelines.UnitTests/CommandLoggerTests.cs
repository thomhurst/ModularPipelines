using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests;

public class CommandLoggerTests : TestBase
{
    [Test]
    [Combinatorial]
    public async Task Logs_As_Expected_With_Options(
        [Values(true, false)] bool logInput,
        [Values(true, false)] bool logOutput,
        [Values(true, false)] bool logError)
    {
        var file = await RunPowershellCommand("""
                                        echo Hello world!
                                        throw "Error!"
                                        """, logInput, logOutput, logError);

        var logFile = await File.ReadAllTextAsync(file);

        if (!logInput && !logOutput && !logError)
        {
            Assert.That(logFile, Does.Not.Contain("INFO	[ModularPipelines.Logging.CommandLogger]"));
            return;
        }

        Assert.That(logFile, Does.Contain("INFO	[ModularPipelines.Logging.CommandLogger]"));

        Assert.That(logFile, logInput
            ? Does.Contain("""
                           ---Executing Command---
                           pwsh -Command "echo Hello world!
                           throw \"Error!\""
                           """)
            : Does.Contain("""
                           ---Executing Command---
                           ********
                           """));

        Assert.That(logFile,
            logOutput ? Does.Contain("---Command Result---") : Does.Not.Contain("---Command Result---"));

        Assert.That(logFile,
            logError ? Does.Contain("---Command Error") : Does.Not.Contain("---Command Error"));
    }

    private async Task<string> RunPowershellCommand(string command, bool logInput, bool logOutput, bool logError)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Guid.NewGuid().ToString("N") + ".txt");

        var result = await GetService<ICommand>((_, collection) =>
        {
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

        await result.T.ExecuteCommandLineTool(new PowershellScriptOptions(command)
        {
            CommandLogging = logging,
            ThrowOnNonZeroExitCode = false,
        });

        await result.Host.DisposeAsync();

        return file;
    }
}