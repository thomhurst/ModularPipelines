using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.GitHub.PipelineWriters;
using ModularPipelines.TestHelpers;
using ModularPipelines.FileSystem;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class DistributedPipelineWriterTests : TestBase
{
    [Test]
    public async Task GeneratesMatrixFromModuleCapabilities()
    {
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "nested",
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<LinuxModule>()
            .AddModule<WindowsModule>()
            .AddModule<MacOrWindowsModule>()
            .AddModule<CustomCapabilityModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                PipelineProjectPath = new FilePath("src/MyPipeline"),
                DotNetRunFramework = "net10.0",
                ExtraWorkers = 1,
            })
            .RunAsync();

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
        await Assert.That(yaml).Contains("needs: initialize");
        await Assert.That(yaml).Contains(
            "run-identifier: ${{ steps.identifier.outputs.value }}");
        await Assert.That(yaml).Contains(
            "run: echo \"value=${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}\" >> \"$GITHUB_OUTPUT\"");
        await Assert.That(yaml).Contains(
            "RUN_IDENTIFIER: ${{ needs.initialize.outputs.run-identifier }}");
        await Assert.That(yaml).Contains("name: Validate retry scope");
        await Assert.That(yaml).Contains(
            "if [ \"${{ needs.initialize.outputs.run-identifier }}\" != \"${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}\" ]; then");
        await Assert.That(yaml).Contains(
            "Distributed workflows require 'Re-run all jobs'; partial retries cannot recreate the worker matrix.");
        await Assert.That(yaml).Contains(
            "run: dotnet run --project 'src/MyPipeline' -c Release --framework net10.0");
        var lines = yaml.Split('\n');
        var runPipelineStepIndex = Array.FindIndex(
            lines,
            line => line.Trim() == "- name: Run Pipeline");
        await Assert.That(runPipelineStepIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(lines[runPipelineStepIndex + 1].Trim()).IsEqualTo("shell: bash");
        await Assert.That(yaml).DoesNotContain("pull_request:");
    }

    [Test]
    public async Task GeneratesRunnersForOperatingSystemConditions()
    {
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<WindowsConditionModule>()
            .AddModule<MacConditionModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                ExtraWorkers = 0,
            })
            .RunAsync();

        var yaml = (await outputPath.ReadAsync()).ReplaceLineEndings("\n");
        var runners = yaml.Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.StartsWith("os:", StringComparison.Ordinal));

        await Assert.That(runners).IsEquivalentTo(
            ["os: ubuntu-latest", "os: windows-latest", "os: macos-latest"]);
    }

    [Test]
    public async Task RejectsUnsupportedOperatingSystemConditions()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<FreeBsdConditionModule>()
                .WriteDistributedWorkflow(new DistributedWorkflowOptions
                {
                    OutputPath = FilePath.GetNewTemporaryFilePath(),
                    ExtraWorkers = 0,
                })
                .RunAsync());

        await Assert.That(exception!.Message).Contains("freebsd");
    }

    [Test]
    public async Task QuotesPortablePipelineProjectPath()
    {
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<LinuxModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                PipelineProjectPath = new FilePath(@"src\My Pipeline's\Pipeline.csproj"),
            })
            .RunAsync();

        var yaml = (await outputPath.ReadAsync()).ReplaceLineEndings("\n");

        await Assert.That(yaml).Contains(
            "run: dotnet run --project 'src/My Pipeline'\"'\"'s/Pipeline.csproj' -c Release");
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

    [RunIf<OnWindows>]
    private sealed class WindowsConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<OnMacOS>]
    private sealed class MacConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<OnFreeBSD>]
    private sealed class FreeBsdConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
