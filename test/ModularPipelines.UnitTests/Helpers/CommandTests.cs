using System.Diagnostics;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class CommandTests : TestBase
{
    [Test]
    public async Task Command_Execution_Default_Timeout_Is_Thirty_Minutes()
    {
        var executionOptions = new CommandExecutionOptions();

        await Assert.That(CommandExecutionOptions.DefaultExecutionTimeout)
            .IsEqualTo(TimeSpan.FromMinutes(30));
        await Assert.That(executionOptions.ExecutionTimeout)
            .IsEqualTo(CommandExecutionOptions.DefaultExecutionTimeout);
        await Assert.That(executionOptions.MaxCapturedOutputLength)
            .IsEqualTo(CommandExecutionOptions.DefaultMaxCapturedOutputLength);
    }

    [Test]
    public async Task Command_Execution_Timeout_Can_Be_Overridden()
    {
        var timeout = TimeSpan.FromMinutes(5);
        var executionOptions = new CommandExecutionOptions { ExecutionTimeout = timeout };

        await Assert.That(executionOptions.ExecutionTimeout).IsEqualTo(timeout);
    }

    [Test]
    public async Task Command_Execution_Caps_Captured_Output_With_Head_And_Tail()
    {
        var command = await GetService<ICommand>();
        var result = await command.ExecuteCommandLineTool(
            new PowershellScriptOptions("Write-Output '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'"),
            new CommandExecutionOptions { MaxCapturedOutputLength = 10 });

        using (Assert.Multiple())
        {
            await Assert.That(result.StandardOutput).StartsWith("01234");
            await Assert.That(result.StandardOutput).Contains("truncated");
            await Assert.That(result.StandardOutput).Contains("XYZ");
            await Assert.That(result.StandardOutput).EndsWith(Environment.NewLine);
            await Assert.That(result.StandardOutput.Length).IsLessThan(100);
        }
    }

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
            var command = await GetService<ICommandContext>();

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
            await Assert.That(result.EnvironmentVariables["PATH"]).IsEqualTo(tempDirectory);
            await Assert.That(result.EnvironmentVariables.Keys.Any(key =>
                key.StartsWith("MODULAR_PIPELINES_CMD_", StringComparison.OrdinalIgnoreCase))).IsFalse();
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
            var command = await GetService<ICommandContext>();

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
    public async Task ExecuteCommandLineTool_Resolves_Extensionless_Relative_Windows_Command_Script()
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
            var command = await GetService<ICommandContext>();
            var relativeToolPath = Path.ChangeExtension(
                Path.GetRelativePath(Environment.CurrentDirectory, scriptPath),
                null);

            var resolvedScript = WindowsCommandResolver.Resolve(
                relativeToolPath,
                Environment.CurrentDirectory,
                pathExtensions: ".COM;.EXE;.BAT;.CMD",
                isWindows: true);

            var result = await command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions(relativeToolPath),
                new CommandExecutionOptions
                {
                    WorkingDirectory = tempDirectory,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATHEXT"] = ".COM;.EXE;.BAT;.CMD",
                    },
                });

            await Assert.That(resolvedScript).IsEqualTo(Path.GetFullPath(scriptPath));
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
            var command = await GetService<ICommandContext>();

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

    [Test]
    public async Task ExecuteCommandLineTool_ForcefulCancellation_KillsDescendantProcesses()
    {
        var pidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-child-{Guid.NewGuid():N}.pid");
        Process? childProcess = null;

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script = string.Join(
                "; ",
                "$child = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(pidFile)}' -Value $child.Id",
                "Wait-Process -Id $child.Id");

            var executionTask = command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(50),
                },
                cancellationTokenSource.Token);

            using var pidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var childProcessId = await WaitForProcessIdAsync(pidFile, pidFileTimeout.Token);
            childProcess = Process.GetProcessById(childProcessId);
            cancellationTokenSource.Cancel();

            await Assert.ThrowsAsync<CommandException>(async () => await executionTask);

            var childExited = await WaitForExitAsync(childProcess, TimeSpan.FromSeconds(2));
            await Assert.That(childExited).IsTrue();
        }
        finally
        {
            if (childProcess is { HasExited: false })
            {
                childProcess.Kill(entireProcessTree: true);
                await childProcess.WaitForExitAsync();
            }

            childProcess?.Dispose();
            File.Delete(pidFile);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_ForcefulCancellation_KillsDescendantAfterParentExits()
    {
        var fileSuffix = Guid.NewGuid().ToString("N");
        var pidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-child-{fileSuffix}.pid");
        var parentExitFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-parent-exit-{fileSuffix}");
        Process? childProcess = null;

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script = string.Join(
                "; ",
                "$child = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(pidFile)}' -Value $child.Id",
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(parentExitFile)}')) {{ Start-Sleep -Milliseconds 10 }}");

            var executionTask = command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromMilliseconds(100),
                },
                cancellationTokenSource.Token);

            using var pidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var childProcessId = await WaitForProcessIdAsync(pidFile, pidFileTimeout.Token);
            childProcess = Process.GetProcessById(childProcessId);
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(parentExitFile, string.Empty);

            await Assert.ThrowsAsync<CommandException>(async () => await executionTask);

            var childExited = await WaitForExitAsync(childProcess, TimeSpan.FromSeconds(2));
            await Assert.That(childExited).IsTrue();
        }
        finally
        {
            if (childProcess is { HasExited: false })
            {
                childProcess.Kill(entireProcessTree: true);
                await childProcess.WaitForExitAsync();
            }

            childProcess?.Dispose();
            File.Delete(pidFile);
            File.Delete(parentExitFile);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_GracefulExit_DoesNotWaitForForcefulTimeout()
    {
        var parentExitFile = Path.Combine(
            Path.GetTempPath(),
            $"modular-pipelines-parent-exit-{Guid.NewGuid():N}");

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var script =
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(parentExitFile)}')) " +
                "{ Start-Sleep -Milliseconds 10 }";

            var executionTask = command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", script],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromSeconds(10),
                },
                cancellationTokenSource.Token);

            await Task.Delay(100);
            var stopwatch = Stopwatch.StartNew();
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(parentExitFile, string.Empty);

            await Assert.ThrowsAsync<CommandException>(async () => await executionTask);
            await Assert.That(stopwatch.Elapsed).IsLessThan(TimeSpan.FromSeconds(5));
        }
        finally
        {
            File.Delete(parentExitFile);
        }
    }

    [Test]
    public async Task ExecuteCommandLineTool_ForcefulCancellation_CapturesDescendantSpawnedDuringGrace()
    {
        var fileSuffix = Guid.NewGuid().ToString("N");
        var triggerFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-trigger-{fileSuffix}");
        var intermediatePidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-intermediate-{fileSuffix}.pid");
        var intermediateReadyFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-intermediate-{fileSuffix}.ready");
        var grandchildPidFile = Path.Combine(Path.GetTempPath(), $"modular-pipelines-grandchild-{fileSuffix}.pid");
        Process? intermediateProcess = null;
        Process? grandchildProcess = null;

        try
        {
            var command = await GetService<ICommandContext>();
            using var cancellationTokenSource = new CancellationTokenSource();
            var intermediateScript = string.Join(
                "; ",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(intermediateReadyFile)}' -Value 'ready'",
                $"while (-not (Test-Path -LiteralPath '{EscapePowerShellLiteral(triggerFile)}')) {{ Start-Sleep -Milliseconds 10 }}",
                "$grandchild = Start-Process pwsh -ArgumentList '-NoProfile', '-Command', 'Start-Sleep -Seconds 60' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(grandchildPidFile)}' -Value $grandchild.Id",
                "Start-Sleep -Milliseconds 500");
            var encodedIntermediateScript =
                Convert.ToBase64String(Encoding.Unicode.GetBytes(intermediateScript));
            var parentScript = string.Join(
                "; ",
                $"$intermediate = Start-Process pwsh -ArgumentList '-NoProfile', '-EncodedCommand', '{encodedIntermediateScript}' -PassThru",
                $"Set-Content -LiteralPath '{EscapePowerShellLiteral(intermediatePidFile)}' -Value $intermediate.Id",
                "Wait-Process -Id $intermediate.Id");

            var executionTask = command.ExecuteCommandLineTool(
                new GenericCommandLineToolOptions("pwsh")
                {
                    Arguments = ["-NoProfile", "-Command", parentScript],
                },
                new CommandExecutionOptions
                {
                    GracefulShutdownTimeout = TimeSpan.FromSeconds(1),
                },
                cancellationTokenSource.Token);

            using var intermediatePidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var intermediateProcessId = await WaitForProcessIdAsync(
                intermediatePidFile,
                intermediatePidFileTimeout.Token);
            intermediateProcess = Process.GetProcessById(intermediateProcessId);
            await WaitForFileAsync(intermediateReadyFile, intermediatePidFileTimeout.Token);
            cancellationTokenSource.Cancel();
            await File.WriteAllTextAsync(triggerFile, string.Empty);

            using var grandchildPidFileTimeout = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            var grandchildProcessId = await WaitForProcessIdAsync(
                grandchildPidFile,
                grandchildPidFileTimeout.Token);
            grandchildProcess = Process.GetProcessById(grandchildProcessId);
            await Assert.ThrowsAsync<CommandException>(async () => await executionTask);

            var grandchildExited = await WaitForExitAsync(grandchildProcess, TimeSpan.FromSeconds(2));
            await Assert.That(grandchildExited).IsTrue();
        }
        finally
        {
            foreach (var process in new[] { intermediateProcess, grandchildProcess })
            {
                if (process is { HasExited: false })
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync();
                }

                process?.Dispose();
            }

            File.Delete(triggerFile);
            File.Delete(intermediatePidFile);
            File.Delete(intermediateReadyFile);
            File.Delete(grandchildPidFile);
        }
    }

    private static string EscapePowerShellLiteral(string value) => value.Replace("'", "''");

    private static async Task<int> WaitForProcessIdAsync(string pidFile, CancellationToken cancellationToken)
    {
        while (true)
        {
            if (File.Exists(pidFile))
            {
                try
                {
                    var processId = await File.ReadAllTextAsync(pidFile, cancellationToken);
                    if (int.TryParse(processId.Trim(), out var parsedProcessId))
                    {
                        return parsedProcessId;
                    }
                }
                catch (IOException)
                {
                    // The shell may still be creating or writing the PID file.
                }
            }

            await Task.Delay(20, cancellationToken);
        }
    }

    private static async Task WaitForFileAsync(string path, CancellationToken cancellationToken)
    {
        while (!File.Exists(path))
        {
            await Task.Delay(20, cancellationToken);
        }
    }

    private static async Task<bool> WaitForExitAsync(Process process, TimeSpan timeout)
    {
        try
        {
            await process.WaitForExitAsync().WaitAsync(timeout);
            return true;
        }
        catch (TimeoutException)
        {
            return false;
        }
    }
}
