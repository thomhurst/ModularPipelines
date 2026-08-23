using System.Diagnostics;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ProcessCliCommandExecutorTests
{
    [Test]
    public async Task ExecutableOverrideVariable_Is_Matrix_Scoped()
    {
        await Assert.That(ProcessCliCommandExecutor.ExecutableOverrideVariableName)
            .IsEqualTo("MODULARPIPELINES_CLI_EXECUTABLE");
    }

    [Test]
    public async Task DescendantIdentity_Rejects_Process_Older_Than_Root()
    {
        var rootStart = new DateTime(2026, 8, 3, 12, 0, 0, DateTimeKind.Utc);

        using (Assert.Multiple())
        {
            await Assert.That(DescendantProcessTracker.CanBeDescendant(
                    rootStart,
                    rootStart.AddTicks(-1)))
                .IsFalse();
            await Assert.That(DescendantProcessTracker.CanBeDescendant(
                    rootStart,
                    rootStart))
                .IsTrue();
        }
    }

    [Test]
    public async Task DescendantIdentity_Rejects_Child_Created_After_Parent_Exit()
    {
        var parentStart = new DateTime(2026, 8, 3, 12, 0, 0, DateTimeKind.Utc);
        var parentExit = parentStart.AddSeconds(1);

        using (Assert.Multiple())
        {
            await Assert.That(DescendantProcessTracker.CanBeChildOfParent(
                    parentStart,
                    parentExit,
                    parentStart))
                .IsTrue();
            await Assert.That(DescendantProcessTracker.CanBeChildOfParent(
                    parentStart,
                    parentExit,
                    parentExit))
                .IsTrue();
            await Assert.That(DescendantProcessTracker.CanBeChildOfParent(
                    parentStart,
                    parentExit,
                    parentExit.AddTicks(1)))
                .IsFalse();
        }
    }

    [Test]
    public async Task DescendantIdentity_Stops_When_Exited_Parent_Has_No_Exit_Time()
    {
        var exitTime = new DateTime(2026, 8, 3, 12, 0, 1, DateTimeKind.Utc);

        using (Assert.Multiple())
        {
            await Assert.That(DescendantProcessTracker.CanCaptureChildren(
                    hasExited: false,
                    exitTime: null))
                .IsTrue();
            await Assert.That(DescendantProcessTracker.CanCaptureChildren(
                    hasExited: true,
                    exitTime: exitTime))
                .IsTrue();
            await Assert.That(DescendantProcessTracker.CanCaptureChildren(
                    hasExited: true,
                    exitTime: null))
                .IsFalse();
        }
    }

    [Test]
    public async Task Deferred_Kill_Requires_Matching_Process_Identity()
    {
        var expectedStart = new DateTime(2026, 8, 3, 12, 0, 0, DateTimeKind.Utc);

        using (Assert.Multiple())
        {
            await Assert.That(ProcessCliCommandExecutor.MatchesProcessIdentity(
                    expectedStart,
                    expectedStart))
                .IsTrue();
            await Assert.That(ProcessCliCommandExecutor.MatchesProcessIdentity(
                    expectedStart,
                    expectedStart.AddTicks(1)))
                .IsFalse();
            await Assert.That(ProcessCliCommandExecutor.MatchesProcessIdentity(
                    expectedStartTime: null,
                    actualStartTime: expectedStart))
                .IsFalse();
        }
    }

    [Test]
    public async Task ExecutableOverride_Applies_To_Matching_Command()
    {
        var applies = ProcessCliCommandExecutor.IsOverrideForCommand(
            "podman",
            "/usr/bin/podman");

        await Assert.That(applies).IsTrue();
    }

    [Test]
    public async Task ExecutableOverride_Does_Not_Redirect_Helper_Command()
    {
        var applies = ProcessCliCommandExecutor.IsOverrideForCommand(
            "/tmp/docker-compose",
            "/usr/bin/podman");

        await Assert.That(applies).IsFalse();
    }

    [Test]
    public async Task Resolves_Each_Path_Directory_Before_Trying_The_Next_Extension()
    {
        var root = Path.Combine(Path.GetTempPath(), "mp-cli-executor-tests", Guid.NewGuid().ToString("N"));
        var firstDirectory = Path.Combine(root, "first");
        var secondDirectory = Path.Combine(root, "second");

        try
        {
            Directory.CreateDirectory(firstDirectory);
            Directory.CreateDirectory(secondDirectory);

            var firstBatchFile = Path.Combine(firstDirectory, "gradle.bat");
            var secondExecutable = Path.Combine(secondDirectory, "gradle.exe");
            await File.WriteAllTextAsync(firstBatchFile, string.Empty);
            await File.WriteAllTextAsync(secondExecutable, string.Empty);

            var resolved = ProcessCliCommandExecutor.ResolveExecutablePath(
                "gradle",
                string.Join(Path.PathSeparator, firstDirectory, secondDirectory),
                string.Join(Path.PathSeparator, ".EXE", ".BAT"),
                isWindows: true);

            await Assert.That(resolved).IsEqualTo(Path.GetFullPath(firstBatchFile));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Test]
    public async Task Resolves_Pathext_For_Explicit_Extensionless_Path()
    {
        var root = Path.Combine(Path.GetTempPath(), "mp-cli-executor-tests", Guid.NewGuid().ToString("N"));
        var scriptDirectory = Path.Combine(root, "scripts");
        var scriptPath = Path.Combine(scriptDirectory, "tool.cmd");

        try
        {
            Directory.CreateDirectory(scriptDirectory);
            await File.WriteAllTextAsync(scriptPath, string.Empty);

            var resolved = ProcessCliCommandExecutor.ResolveExecutablePath(
                Path.Combine("scripts", "tool"),
                searchPath: string.Empty,
                pathExtensions: ".CMD",
                isWindows: true,
                processDirectory: root);

            await Assert.That(resolved).IsEqualTo(Path.GetFullPath(scriptPath));
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Test]
    public async Task Uses_Command_Interpreter_For_Windows_Batch_Files()
    {
        var startInfo = ProcessCliCommandExecutor.CreateStartInfo(
            @"C:\Program Files\Gradle\gradle.bat",
            "--help",
            isWindows: true,
            commandInterpreter: @"C:\Windows\System32\cmd.exe");

        await Assert.That(startInfo.FileName).IsEqualTo(@"C:\Windows\System32\cmd.exe");
        await Assert.That(startInfo.Arguments).IsEqualTo("/d /s /c \"\"C:\\Program Files\\Gradle\\gradle.bat\" --help\"");
        await Assert.That(startInfo.UseShellExecute).IsFalse();
        await Assert.That(startInfo.RedirectStandardOutput).IsTrue();
        await Assert.That(startInfo.RedirectStandardError).IsTrue();
        await Assert.That(startInfo.RedirectStandardInput).IsTrue();
    }

    [Test]
    public async Task Timeout_Returns_When_Command_Has_Long_Running_Child()
    {
        var childPidPath = Path.Combine(
            Path.GetTempPath(),
            $"mp-cli-child-{Guid.NewGuid():N}.pid");
        string? scriptPath = null;
        int? childPid = null;
        try
        {
            var command = await CreateLongRunningChildCommandAsync(
                childPidPath,
                parentExits: false);
            scriptPath = command.ScriptPath;
            var executor = new ProcessCliCommandExecutor(
                NullLogger<ProcessCliCommandExecutor>.Instance,
                OperatingSystem.IsWindows()
                    ? TimeSpan.FromMilliseconds(1500)
                    : TimeSpan.FromSeconds(2));

            var execution = executor.ExecuteAsync(command.Command, command.Arguments);
            var result = await execution.WaitAsync(TimeSpan.FromSeconds(5));
            childPid = int.Parse(await File.ReadAllTextAsync(childPidPath));

            using (Assert.Multiple())
            {
                await Assert.That(result.ExitCode).IsEqualTo(-1);
                await Assert.That(result.StandardError).Contains("timed out");
                await Assert.That(await WaitForProcessExitAsync(childPid.Value)).IsTrue();
            }
        }
        finally
        {
            if (childPid.HasValue && IsProcessRunning(childPid.Value))
            {
                using var childProcess = Process.GetProcessById(childPid.Value);
                childProcess.Kill();
            }

            File.Delete(childPidPath);
            if (scriptPath is not null)
            {
                File.Delete(scriptPath);
            }
        }
    }

    [Test]
    public async Task Timeout_Returns_When_Exited_Command_Leaves_Pipe_Open()
    {
        var childPidPath = Path.Combine(
            Path.GetTempPath(),
            $"mp-cli-exited-parent-child-{Guid.NewGuid():N}.pid");
        string? scriptPath = null;
        int? childPid = null;
        try
        {
            var command = await CreateLongRunningChildCommandAsync(
                childPidPath,
                parentExits: true);
            scriptPath = command.ScriptPath;
            var executor = new ProcessCliCommandExecutor(
                NullLogger<ProcessCliCommandExecutor>.Instance,
                OperatingSystem.IsWindows()
                    ? TimeSpan.FromMilliseconds(1500)
                    : TimeSpan.FromSeconds(2));

            var result = await executor.ExecuteAsync(command.Command, command.Arguments)
                .WaitAsync(TimeSpan.FromSeconds(5));
            childPid = int.Parse(await File.ReadAllTextAsync(childPidPath));

            using (Assert.Multiple())
            {
                await Assert.That(result.ExitCode).IsEqualTo(-1);
                await Assert.That(result.StandardError).Contains("timed out");
                await Assert.That(await WaitForProcessExitAsync(childPid.Value)).IsTrue();
            }
        }
        finally
        {
            if (childPid.HasValue && IsProcessRunning(childPid.Value))
            {
                using var childProcess = Process.GetProcessById(childPid.Value);
                childProcess.Kill();
            }

            File.Delete(childPidPath);
            if (scriptPath is not null)
            {
                File.Delete(scriptPath);
            }
        }
    }

    [Test]
    public async Task Executes_Windows_Batch_Files_With_Redirected_Output()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var root = Path.Combine(Path.GetTempPath(), "mp-cli-executor-tests", Guid.NewGuid().ToString("N"));
        var batchFile = Path.Combine(root, "echo arguments.cmd");

        try
        {
            Directory.CreateDirectory(root);
            await File.WriteAllTextAsync(batchFile, "@echo off\r\necho %*\r\n");

            var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);
            var result = await executor.ExecuteAsync(batchFile, "one two");

            await Assert.That(result.ExitCode).IsEqualTo(0);
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo("one two");
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Test]
    public async Task Executes_Quoted_Windows_Batch_Arguments()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var tempDirectory = Path.Combine(Path.GetTempPath(), "mp command script tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDirectory);
        var scriptPath = Path.Combine(tempDirectory, "echo argument.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %~1\r\n");
            var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

            var result = await executor.ExecuteAsync(scriptPath, "\"hello world\"");

            await Assert.That(result.ExitCode).IsEqualTo(0)
                .Because($"stdout: {result.StandardOutput}; stderr: {result.StandardError}");
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo("hello world");
        }
        finally
        {
            Directory.Delete(tempDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Resolves_Relative_Windows_Command_Scripts_Before_Changing_Working_Directory()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var workingDirectory = Path.Combine(Path.GetTempPath(), "mp command script tests", Guid.NewGuid().ToString("N"));
        var scriptDirectory = Path.Combine(Environment.CurrentDirectory, $"mp-generator-relative-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);
        Directory.CreateDirectory(scriptDirectory);
        var scriptPath = Path.Combine(scriptDirectory, "echo-working-directory.cmd");

        try
        {
            await File.WriteAllTextAsync(scriptPath, "@echo off\r\necho %CD%\r\n");
            var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

            var result = await executor.ExecuteAsync(
                Path.GetRelativePath(Environment.CurrentDirectory, scriptPath),
                string.Empty,
                workingDirectory: workingDirectory);

            await Assert.That(result.ExitCode).IsEqualTo(0)
                .Because($"stdout: {result.StandardOutput}; stderr: {result.StandardError}");
            await Assert.That(result.StandardOutput.Trim()).IsEqualTo(workingDirectory);
        }
        finally
        {
            Directory.Delete(workingDirectory, recursive: true);
            Directory.Delete(scriptDirectory, recursive: true);
        }
    }

    [Test]
    public async Task IsAvailableAsync_Returns_False_For_Missing_Windows_Command_Script()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

        var isAvailable = await executor.IsAvailableAsync($"missing-{Guid.NewGuid():N}.cmd");

        await Assert.That(isAvailable).IsFalse();
    }

    [Test]
    public async Task Argument_Aware_IsAvailableAsync_Falls_Back_To_Help()
    {
        var root = Path.Combine(Path.GetTempPath(), "mp-cli-executor-tests", Guid.NewGuid().ToString("N"));
        var scriptPath = Path.Combine(root, OperatingSystem.IsWindows() ? "probe.cmd" : "probe.sh");

        try
        {
            Directory.CreateDirectory(root);
            if (OperatingSystem.IsWindows())
            {
                await File.WriteAllTextAsync(
                    scriptPath,
                    "@echo off\r\nif \"%~1\"==\"--help\" exit /b 0\r\nexit /b 1\r\n");
            }
            else
            {
                await File.WriteAllTextAsync(
                    scriptPath,
                    "#!/bin/sh\n[ \"$1\" = \"--help\" ]\n");
                File.SetUnixFileMode(
                    scriptPath,
                    UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
            }

            var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

            var isAvailable = await executor.IsAvailableAsync(scriptPath, "--version");

            await Assert.That(isAvailable).IsTrue();
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    [Test]
    public async Task IsAvailableAsync_Returns_False_For_Missing_Command()
    {
        var executor = new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance);

        var isAvailable = await executor.IsAvailableAsync($"missing-{Guid.NewGuid():N}");

        await Assert.That(isAvailable).IsFalse();
    }

    private static async Task<bool> WaitForProcessExitAsync(int processId)
    {
        var timeout = DateTimeOffset.UtcNow + TimeSpan.FromSeconds(2);
        while (DateTimeOffset.UtcNow < timeout)
        {
            if (!IsProcessRunning(processId))
            {
                return true;
            }

            await Task.Delay(TimeSpan.FromMilliseconds(50));
        }

        return !IsProcessRunning(processId);
    }

    private static async Task<(string Command, string Arguments, string? ScriptPath)>
        CreateLongRunningChildCommandAsync(string childPidPath, bool parentExits)
    {
        if (!OperatingSystem.IsWindows())
        {
            var parentCommand = parentExits ? "sleep 0.05" : "wait";
            return (
                "/bin/sh",
                $"-c \"sleep 30 & child=$!; echo $child > '{childPidPath}'; {parentCommand}\"",
                null);
        }

        var scriptPath = Path.ChangeExtension(childPidPath, ".cmd");
        var escapedPidPath = childPidPath.Replace("'", "''", StringComparison.Ordinal);
        var parentDelayMilliseconds = parentExits ? 0 : 3000;
        await File.WriteAllTextAsync(
            scriptPath,
            $"""
            @echo off
            start "" /b powershell.exe -NoProfile -Command "$PID | Set-Content -LiteralPath '{escapedPidPath}'; Start-Sleep -Seconds 30"
            powershell.exe -NoProfile -Command "Start-Sleep -Milliseconds {parentDelayMilliseconds}"
            """);
        return (scriptPath, string.Empty, scriptPath);
    }

    private static bool IsProcessRunning(int processId)
    {
        try
        {
            using var process = Process.GetProcessById(processId);
            return !process.HasExited;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
