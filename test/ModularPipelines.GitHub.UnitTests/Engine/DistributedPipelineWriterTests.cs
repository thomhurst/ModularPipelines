using ModularPipelines.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.GitHub.PipelineWriters;
using ModularPipelines.TestHelpers;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class DistributedPipelineWriterTests : TestBase
{
    [Test]
    public async Task GeneratesMatrixFromModuleCapabilities()
    {
        var outputPath = new File(Path.Combine(
            File.GetNewTemporaryFilePath().Path,
            "nested",
            "distributed.yml"));

        await TestPipelineHostBuilder.Create()
            .AddModule<LinuxModule>()
            .AddModule<WindowsModule>()
            .AddModule<MacOrWindowsModule>()
            .AddModule<CustomCapabilityModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                PipelineProjectPath = new File("src/MyPipeline"),
                DotNetRunFramework = "net10.0",
                ExtraWorkers = 1,
            })
            .ExecutePipelineAsync();

        var yaml = (await outputPath.ReadAsync()).ReplaceLineEndings("\n");

        await Assert.That(yaml).Contains("uses: actions/checkout@v7.0.1");
        await Assert.That(yaml).Contains("uses: actions/setup-dotnet@v6.0.0");
        await Assert.That(yaml).Contains("uses: actions/cache@v6.1.0");
        await Assert.That(yaml).Contains("dotnet-version: 10.0.x");
        await Assert.That(yaml).Contains("runs-on: ${{ matrix.os }}");
        var matrixLines = yaml.Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.StartsWith("- instance:", StringComparison.Ordinal)
                           || line.StartsWith("os:", StringComparison.Ordinal));
        await Assert.That(string.Join('|', matrixLines)).IsEqualTo(
            "- instance: 0|os: ubuntu-latest|"
            + "- instance: 1|os: ubuntu-latest|"
            + "- instance: 2|os: windows-latest|"
            + "- instance: 3|os: macos-latest|"
            + "- instance: 4|os: ubuntu-latest");
        await Assert.That(yaml).Contains("INSTANCE_INDEX: ${{ matrix.instance }}");
        await Assert.That(yaml).Contains("TOTAL_INSTANCES: 5");
        await Assert.That(yaml).Contains("REDIS_URL: ${{ secrets.REDIS_URL }}");
        await Assert.That(yaml).Contains(
            "run: dotnet run --project src/MyPipeline -c Release --framework net10.0");
    }

    [RequiresCapability("linux")]
    private sealed class LinuxModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresCapability("windows")]
    private sealed class WindowsModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresCapability("operating-system:windows|macos")]
    private sealed class MacOrWindowsModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresCapability("docker")]
    private sealed class CustomCapabilityModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
