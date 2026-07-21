using ModularPipelines.Helpers.Internal;
using ModularPipelines.Java.Enums;
using ModularPipelines.Java.Options;
using ModularPipelines.Models;

namespace ModularPipelines.UnitTests.Attributes;

public class JavaOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

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

        await Assert.That(arguments).IsEquivalentTo(
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

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--console", "plain",
            "--project-prop", "environment=ci",
            "--max-workers", "4",
            "--no-daemon",
            "clean",
            "build",
        ]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
