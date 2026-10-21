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
    [Arguments("tool")]
    [Arguments("tool.exe")]
    public async Task Unresolved_Windows_Command_Does_Not_Fall_Back_To_Operating_System_Search(string tool)
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
