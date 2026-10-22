using System.ComponentModel;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Helpers;

public class CliCommandFactoryTests
{
    [Test]
    public async Task Direct_Command_Starts_Executable_Found_On_Command_Path()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Path.Combine(Path.GetTempPath(), "mp factory tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        var pathDirectory = Directory.CreateDirectory(Path.Combine(directory, "bin")).FullName;
        var executable = Path.Combine(pathDirectory, "mp-factory-tool.exe");

        try
        {
            await File.WriteAllBytesAsync(executable, []);
            await File.WriteAllBytesAsync(Path.Combine(directory, "mp-factory-tool.exe"), []);

            var prepared = CliCommandFactory.Create(
                "mp-factory-tool",
                ["--flag"],
                new CommandExecutionOptions
                {
                    WorkingDirectory = directory,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = pathDirectory,
                        ["PATHEXT"] = ".EXE;.CMD",
                    },
                });

            using (Assert.Multiple())
            {
                await Assert.That(prepared.Command.ResolvedFilePath).IsEqualTo(executable);
                await Assert.That(prepared.Command.CreateStartInfo().FileName).IsEqualTo(executable);
                await Assert.That(prepared.Input).IsEqualTo("mp-factory-tool --flag");
            }
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }

    [Test]
    [TUnit.Core.NotInParallel]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Windows_Script_Preserves_Configured_Comspec(bool useAbsolutePath)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Directory.CreateTempSubdirectory("mp configured host tests ");
        var host = Path.Combine(directory.FullName, "configured-host.exe");
        var script = Path.Combine(directory.FullName, "script.cmd");
        var originalComspec = Environment.GetEnvironmentVariable("COMSPEC");

        try
        {
            await File.WriteAllBytesAsync(host, []);
            await File.WriteAllTextAsync(script, "@echo off\r\n");
            Environment.SetEnvironmentVariable("COMSPEC", useAbsolutePath ? host : "configured-host.exe");

            var prepared = CliCommandFactory.Create(script, [], new CommandExecutionOptions
            {
                WorkingDirectory = directory.FullName,
                EnvironmentVariables = new Dictionary<string, string?>
                {
                    ["PATH"] = directory.FullName,
                    ["PATHEXT"] = ".EXE;.CMD",
                },
            });

            await Assert.That(prepared.Command.CreateStartInfo().FileName).IsEqualTo(host);
        }
        finally
        {
            Environment.SetEnvironmentVariable("COMSPEC", originalComspec);
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Unresolved_Command_Starts_Tool_As_Named()
    {
        if (OperatingSystem.IsWindows())
        {
            return;
        }

        var tool = $"modular-pipelines-missing-{Guid.NewGuid():N}";

        var prepared = CliCommandFactory.Create(tool, []);

        await Assert.That(prepared.Command.CreateStartInfo().FileName).IsEqualTo(tool);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task Explicit_Windows_Path_Does_Not_Require_Current_User_Visibility(bool relativePath, bool useCredentials)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Path.Combine(Path.GetTempPath(), $"mp factory tests {Guid.NewGuid():N}");
        var executable = Path.Combine(directory, "private", "tool.exe");
        var tool = relativePath ? Path.Combine("private", "tool.exe") : executable;

        // An absent file reproduces File.Exists returning false for a file hidden from the current user.
        // No process is started: visibility and credentials must be left to Windows at startup.
        var prepared = CliCommandFactory.Create(tool, [], new CommandExecutionOptions
        {
            WorkingDirectory = directory,
            CommandLineCredentials = useCredentials ? new CommandLineCredentials { UserName = "other-user" } : null,
        });
        var startInfo = prepared.Command.CreateStartInfo();

        using (Assert.Multiple())
        {
            await Assert.That(prepared.Command.ResolvedFilePath).IsNull();
            await Assert.That(startInfo.FileName).IsEqualTo(executable);
            await Assert.That(startInfo.WorkingDirectory).IsEqualTo(directory);
            await Assert.That(startInfo.UserName).IsEqualTo(useCredentials ? "other-user" : string.Empty);
        }
    }

    [Test]
    [Arguments("tool", false)]
    [Arguments("tool.exe", false)]
    [Arguments("tool", true)]
    [Arguments("tool.exe", true)]
    public async Task Unresolved_Windows_Command_Does_Not_Fall_Back_To_Operating_System_Search(string tool, bool useCredentials)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var directory = Directory.CreateTempSubdirectory("mp factory tests ");

        try
        {
            await File.WriteAllBytesAsync(Path.Combine(directory.FullName, "tool.exe"), []);

            var exception = Assert.Throws<Win32Exception>(() => CliCommandFactory.Create(
                tool,
                [],
                new CommandExecutionOptions
                {
                    WorkingDirectory = directory.FullName,
                    CommandLineCredentials = useCredentials ? new CommandLineCredentials { UserName = "other-user" } : null,
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = string.Empty,
                        ["PATHEXT"] = ".EXE;.CMD",
                    },
                }).Command.CreateStartInfo());

            await Assert.That(exception!.NativeErrorCode).IsEqualTo(2);
            await Assert.That(exception.Message).Contains(tool);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
