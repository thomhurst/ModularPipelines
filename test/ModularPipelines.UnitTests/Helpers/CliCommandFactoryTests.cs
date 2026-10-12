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
        var executable = Path.Combine(directory, "mp-factory-tool.exe");

        try
        {
            await File.WriteAllBytesAsync(executable, []);

            var prepared = CliCommandFactory.Create(
                "mp-factory-tool",
                ["--flag"],
                new CommandExecutionOptions
                {
                    EnvironmentVariables = new Dictionary<string, string?>
                    {
                        ["PATH"] = directory,
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
        var tool = $"modular-pipelines-missing-{Guid.NewGuid():N}";

        var prepared = CliCommandFactory.Create(tool, []);

        await Assert.That(prepared.Command.CreateStartInfo().FileName).IsEqualTo(tool);
    }
}
