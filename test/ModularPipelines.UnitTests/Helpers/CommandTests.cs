using System.Diagnostics;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    private class CommandEchoModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.Command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-Command", "echo 'Foo bar!'"],
                },
                cancellationToken: cancellationToken);
        }
    }

    private class CommandEchoTimeoutModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return TestConstants.TestString;
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<CommandEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar_With_Timeout()
    {
        var moduleResult = await await RunModule<CommandEchoTimeoutModule>();

        await Assert.That(moduleResult.ValueOrDefault!.Trim()).IsEqualTo(TestConstants.TestString);
    }

    [Test]
    public async Task ExecuteCommandLineTool_Resolves_Windows_Command_Scripts_From_Path()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "mp-runtime-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %~1\r\n");
            var command = await GetService<ICommand>();

            var result = await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("mp-runtime-test")
                {
                    Arguments = ["hello world"],
                },
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = tempDirectory,
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo("hello world");
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_Preserves_Windows_Command_Script_Metacharacters()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "mp-runtime-metachar-test.cmd");
        const string argument = "value & echo injected | more < input > output %PATH% ^ !PATH!";

        try
        {
            await File.WriteAllTextAsync(
                scriptPath,
                "@echo off\r\nsetlocal DisableDelayedExpansion\r\nset \"arg=%~1\"\r\nsetlocal EnableDelayedExpansion\r\necho(!arg!\r\n");
            var command = await GetService<ICommand>();

            var result = await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions(scriptPath)
                {
                    Arguments = [argument],
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(argument);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_Resolves_Relative_Windows_Command_Script_Before_Changing_Working_Directory()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        var scriptDirectory = Path.Combine(Environment.CurrentDirectory, $"mp-runtime-relative-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDirectory);
        Directory.CreateDirectory(scriptDirectory);
        var scriptPath = Path.Combine(scriptDirectory, "mp-runtime-relative-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %CD%\r\n");
            var command = await GetService<ICommand>();

            var result = await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions(Path.GetRelativePath(Environment.CurrentDirectory, scriptPath)),
                new CommandExecutionOptions
                {
                    WorkingDirectory = tempDirectory,
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(tempDirectory);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
            Directory.Delete(scriptDirectory, recursive: true);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_Resolves_Relative_Path_Entries_Before_Changing_Working_Directory()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var workingDirectory = Path.Combine(Path.GetTempPath(), "mp runtime command tests", Guid.NewGuid().ToString("N"));
        var relativeScriptDirectory = $"mp-runtime-path-{Guid.NewGuid():N}";
        var scriptDirectory = Path.Combine(Environment.CurrentDirectory, relativeScriptDirectory);
        Directory.CreateDirectory(workingDirectory);
        Directory.CreateDirectory(scriptDirectory);
        var scriptPath = Path.Combine(scriptDirectory, "mp-runtime-path-test.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %CD%\r\n");
            var command = await GetService<ICommand>();

            var result = await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("mp-runtime-path-test"),
                new CommandExecutionOptions
                {
                    WorkingDirectory = workingDirectory,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = relativeScriptDirectory,
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(workingDirectory);
            await Assert.That(result.StandardError).IsEmpty();
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
            Directory.Delete(scriptDirectory, recursive: true);
        }
    }
}
