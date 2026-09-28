using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
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
        await Assert.That(yaml).Contains("MODULARPIPELINES_INSTANCE_INDEX: ${{ matrix.instance }}");
        await Assert.That(yaml).Contains("MODULARPIPELINES_TOTAL_INSTANCES: 5");
        await Assert.That(yaml).Contains("REDIS_URL: ${{ secrets.REDIS_URL }}");
        await Assert.That(yaml).Contains("needs: initialize");
        await Assert.That(yaml).Contains(
            "run-identifier: ${{ steps.identifier.outputs.value }}");
        await Assert.That(yaml).Contains(
            "run: echo \"value=${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}\" >> \"$GITHUB_OUTPUT\"");
        await Assert.That(yaml).Contains(
            "MODULARPIPELINES_RUN_ID: ${{ needs.initialize.outputs.run-identifier }}");
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
    public async Task GeneratesRunnersForAlternativeOperatingSystemConditions()
    {
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<UnixConditionModule>()
            .AddModule<FreeBsdOrDockerModule>()
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
            ["os: ubuntu-latest", "os: ubuntu-latest", "os: macos-latest"]);
    }

    [Test]
    public async Task ExcludesRunnersThatConflictWithMandatoryOperatingSystems()
    {
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<WindowsWithConditionalLinuxModule>()
            .AddModule<LinuxWithMacOrDockerModule>()
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

        // Windows is mandatory for the first module and Linux for the second, so no macOS runner.
        await Assert.That(runners).IsEquivalentTo(
            ["os: ubuntu-latest", "os: ubuntu-latest", "os: windows-latest"]);
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
    public async Task RejectsUnsupportedConditionalOperatingSystemRoutes()
    {
        // OnCI is planning-safe; when it is false the master requires a FreeBSD worker.
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<FreeBsdOrCiModule>()
                .WriteDistributedWorkflow(new DistributedWorkflowOptions
                {
                    OutputPath = FilePath.GetNewTemporaryFilePath(),
                    ExtraWorkers = 0,
                })
                .RunAsync());

        await Assert.That(exception!.Message).Contains("freebsd");
    }

    [Test]
    public async Task RejectsOperatingSystemClausesThatOnlyUnsupportedRunnersSatisfyTogether()
    {
        // Each clause lists a supported OS, but only FreeBSD satisfies both.
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<FreeBsdIntersectionModule>()
                .WriteDistributedWorkflow(new DistributedWorkflowOptions
                {
                    OutputPath = FilePath.GetNewTemporaryFilePath(),
                    ExtraWorkers = 0,
                })
                .RunAsync());

        await Assert.That(exception!.Message).Contains("freebsd");
    }

    [Test]
    public async Task ProvisionsConditionalRunnersAcrossManyConditions()
    {
        // Eleven planning-safe conditions; when they are false at run time,
        // the master requires Windows, so the workflow must still provision a Windows runner.
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<ManyConditionalWindowsRoutesModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                ExtraWorkers = 0,
            })
            .RunAsync();

        await Assert.That(await outputPath.ReadAsync()).Contains("windows-latest");
    }

    [Test]
    public async Task RejectsUnsupportedConditionalRoutesAcrossManyConditions()
    {
        // With every OnCI false at run time the master requires FreeBSD, which no runner supports.
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<ManyConditionalFreeBsdRoutesModule>()
                .WriteDistributedWorkflow(new DistributedWorkflowOptions
                {
                    OutputPath = FilePath.GetNewTemporaryFilePath(),
                    ExtraWorkers = 0,
                })
                .RunAsync());

        await Assert.That(exception!.Message).Contains("freebsd");
    }

    [Test]
    public async Task TreatsRepeatedConditionsAsOneValue()
    {
        // Every OnCI reads the same setting: when true nothing is required, and when false the module needs
        // FreeBSD and Windows at once and is skipped. No FreeBSD-only outcome exists, so generation succeeds.
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<CorrelatedConflictingRoutesModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                ExtraWorkers = 0,
            })
            .RunAsync();

        await Assert.That(await outputPath.ReadAsync()).DoesNotContain("windows-latest");
    }

    [Test]
    public async Task KeepsAlternativeRoutesInLargeConditionGroups()
    {
        // The group needs Linux or FreeBSD when OnCI is false; Linux has a runner, so generation succeeds.
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<LargeAlternativeGroupModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                ExtraWorkers = 0,
            })
            .RunAsync();

        await Assert.That(await outputPath.ReadAsync()).Contains("ubuntu-latest");
    }

    [Test]
    public async Task AllowsUnsupportedConditionalRouteWithWorkerOnlyAlternative()
    {
        // A worker-only alternative may hold on any worker, so the route never becomes mandatory.
        var outputPath = new FilePath(Path.Combine(
            FilePath.GetNewTemporaryFilePath().Path,
            "distributed.yml"));

        await TestPipelineBuilder.Create()
            .AddModule<FreeBsdOrWorkerOnlyModule>()
            .WriteDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                ExtraWorkers = 0,
            })
            .RunAsync();

        await Assert.That(await outputPath.ReadAsync()).DoesNotContain("freebsd");
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

    [RequiresCapability("linux", "docker")]
    private sealed class LinuxModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresCapability("windows")]
    private sealed class WindowsModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresAnyCapability("windows", "macos")]
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

    [RequiresCapability("windows")]
    [RunIfAny<OnLinux, OnCI>]
    private sealed class WindowsWithConditionalLinuxModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresCapability("linux")]
    [RequiresAnyCapability("macos", "docker")]
    private sealed class LinuxWithMacOrDockerModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<OnUnix>]
    private sealed class UnixConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    // A worker can satisfy this through docker, so no FreeBSD runner is needed.
    [RequiresAnyCapability("freebsd", "docker")]
    private sealed class FreeBsdOrDockerModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RequiresAnyCapability("linux", "freebsd")]
    [RequiresAnyCapability("windows", "freebsd")]
    private sealed class FreeBsdIntersectionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    private sealed class ManyConditionalWindowsRoutesModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    private sealed class ManyConditionalFreeBsdRoutesModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnFreeBSD, OnCI>]
    [RunIfAny<OnWindows, OnCI>]
    private sealed class CorrelatedConflictingRoutesModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIf<LinuxFreeBsdOrManyCiGroup>]
    private sealed class LargeAlternativeGroupModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class LinuxFreeBsdOrManyCiGroup : ConditionGroup, IPlanningSafe
    {
        public override IReadOnlyList<IRunCondition> Conditions =>
            [new OnLinux(), new OnFreeBSD(), .. Enumerable.Range(0, 13).Select(static _ => new OnCI())];

        public override ConditionLogic Logic => ConditionLogic.Any;
    }

    [RunIfAny<OnFreeBSD, OnCI>]
    private sealed class FreeBsdOrCiModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    [RunIfAny<OnFreeBSD, WorkerOnlyCondition>]
    private sealed class FreeBsdOrWorkerOnlyModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }

    private sealed class WorkerOnlyCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [RunIf<OnFreeBSD>]
    private sealed class FreeBsdConditionModule : SimpleTestModule<bool>
    {
        protected override bool Result => true;
    }
}
