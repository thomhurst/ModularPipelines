using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class ProcessCliCommandExecutorTests
{
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
}
