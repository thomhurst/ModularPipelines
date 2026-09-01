using ModularPipelines.Java.Enums;
using ModularPipelines.Java.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Java.UnitTests.Attributes;

public class JavaOptionsTests
{
    [Test]
    public async Task Maven_Renders_Options_Properties_And_Goals()
    {
        var arguments = BuildArguments(new MavenExecuteOptions
        {
            BatchMode = true,
            Color = MavenColor.Never,
            Define = [new KeyValue("skipTests", "true")],
            GoalsAndPhases = ["clean", "verify"],
        });

        await AssertArguments(arguments,
        [
            "--batch-mode",
            "--color", "never",
            "--define", "skipTests=true",
            "clean",
            "verify",
        ]);
    }

    [Test]
    public async Task Gradle_Renders_Aliases_Typed_Options_And_Tasks()
    {
        var arguments = BuildArguments(new GradleExecuteOptions
        {
            Console = GradleConsole.Plain,
            ProjectProp = [new KeyValue("environment", "ci")],
            NoDaemon = true,
            MaxWorkers = 4,
            Tasks = ["clean", "build"],
        });

        await AssertArguments(arguments,
        [
            "--console", "plain",
            "--project-prop", "environment=ci",
            "--max-workers", "4",
            "--no-daemon",
            "clean",
            "build",
        ]);
    }
}
