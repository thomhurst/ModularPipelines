using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;
using ModularPipelines.GitHub;
using ModularPipelines.TestHelpers;
using ModularPipelines.FileSystem;
using YamlDotNet.RepresentationModel;

namespace ModularPipelines.GitHub.UnitTests.Engine;

public class DistributedPipelineWriterTests : TestBase
{
    [Test]
    [Arguments(true, true)]
    [Arguments(true, false)]
    [Arguments(false, true)]
    [Arguments(false, false)]
    public async Task PublishesOnceAndRunsDownloadedHost(bool cacheNuGet, bool publishEnvironment)
    {
        var outputPath = FilePath.GetNewTemporaryFilePath();
        await TestPipelineBuilder.Create()
            .AddModule<WindowsModule>()
            .AddDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                CacheNuGet = cacheNuGet,
                ExtraWorkers = 0,
                EnvironmentVariables = new Dictionary<string, string> { ["BUILD_FLAVOR"] = "${{ matrix.os }}-${{ matrix.instance }}" },
                PublishEnvironmentVariables = publishEnvironment
                    ? new Dictionary<string, string> { ["BUILD_TOKEN"] = "${{ secrets.BUILD_TOKEN }}" }
                    : null,
            })
            .RunAsync();

        var yaml = new YamlStream();
        yaml.Load(new StringReader(await outputPath.ReadAsync()));
        var jobs = yaml.Documents[0].RootNode["jobs"];
        var initialize = ((YamlSequenceNode) jobs["initialize"]["steps"]).Children.Cast<YamlMappingNode>().ToArray();
        var pipeline = ((YamlSequenceNode) jobs["pipeline"]["steps"]).Children.Cast<YamlMappingNode>().ToArray();
        var publish = initialize.Single(step => step["name"].ToString() == "Publish portable pipeline host");
        await Assert.That(publish["run"].ToString()).Contains("-p:RuntimeIdentifier=");
        await Assert.That(publish["run"].ToString()).Contains("-p:PublishSingleFile=false -p:PublishReadyToRun=false");
        await Assert.That(publish["run"].ToString()).Contains("-p:PublishAot=false -p:PublishTrimmed=false");
        await Assert.That(publish.ToString()).DoesNotContain("matrix.");
        if (publishEnvironment)
        {
            await Assert.That(((YamlMappingNode) publish["env"]).Children.Count).IsEqualTo(1);
            await Assert.That(publish["env"]["BUILD_TOKEN"].ToString()).IsEqualTo("${{ secrets.BUILD_TOKEN }}");
        }
        else
        {
            await Assert.That(publish.ToString()).DoesNotContain("BUILD_TOKEN");
        }

        await Assert.That(string.Join('\n', pipeline.Select(step => step.ToString()))).DoesNotContain("BUILD_TOKEN");
        await Assert.That(initialize.Count(step => step["name"].ToString() == "Cache NuGet")).IsEqualTo(cacheNuGet ? 1 : 0);
        await Assert.That(pipeline.Count(step => step["name"].ToString() == "Cache NuGet")).IsEqualTo(cacheNuGet ? 1 : 0);
        await Assert.That(pipeline.Any(step => step["name"].ToString() == "Publish portable pipeline host")).IsFalse();
        var downloadIndex = Array.FindIndex(pipeline, step => step["name"].ToString() == "Download pipeline host");
        var restoreIndex = Array.FindIndex(pipeline, step => step["name"].ToString() == "Restore runner dependencies");
        var runIndex = Array.FindIndex(pipeline, step => step["name"].ToString() == "Run Pipeline");
        await Assert.That(downloadIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(restoreIndex).IsGreaterThan(downloadIndex);
        await Assert.That(runIndex).IsGreaterThan(restoreIndex);
        await Assert.That(pipeline[restoreIndex]["env"]["BUILD_FLAVOR"].ToString()).IsEqualTo("${{ matrix.os }}-${{ matrix.instance }}");
        await Assert.That(pipeline[runIndex]["run"].ToString()).Contains("--no-build --no-restore");
        await Assert.That(pipeline[runIndex]["env"]["BUILD_FLAVOR"].ToString()).IsEqualTo("${{ matrix.os }}-${{ matrix.instance }}");
    }

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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                PipelineProjectPath = new FilePath("src/MyPipeline"),
                DotNetRunFramework = "net11.0",
                ExtraWorkers = 1,
            })
            .RunAsync();

        var yaml = (await outputPath.ReadAsync()).ReplaceLineEndings("\n");

        await Assert.That(yaml).Contains("uses: actions/checkout@v7.0.1");
        await Assert.That(yaml).Contains("uses: actions/setup-dotnet@v6.0.0");
        await Assert.That(yaml).Contains("uses: actions/cache@v6.1.0");
        await Assert.That(yaml).Contains("dotnet-version: 11.0.x");
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
            "dotnet publish 'src/MyPipeline' -c Release --framework 'net11.0' --no-self-contained -p:UseAppHost=false");
        await Assert.That(yaml).Contains("uses: actions/upload-artifact@v7");
        await Assert.That(yaml).Contains("uses: actions/download-artifact@v8");
        await Assert.That(yaml).Contains("name: modular-pipelines-host-${{ github.run_attempt }}");
        await Assert.That(yaml).Contains("path: ${{ runner.temp }}/modular-pipelines-host");
        await Assert.That(yaml).Contains("if-no-files-found: error");
        await Assert.That(yaml).Contains("include-hidden-files: true");
        await Assert.That(yaml).Contains("name: Restore runner dependencies");
        await Assert.That(yaml).Contains("dotnet restore 'src/MyPipeline' --property:TargetFramework='net11.0'");
        await Assert.That(yaml).Contains(
            "dotnet run --project 'src/MyPipeline' -c Release --framework 'net11.0' --no-build --no-restore -p:UseAppHost=false");
        await Assert.That(yaml).Contains("--property:OutDir=\"$RUNNER_TEMP/modular-pipelines-host/\"");
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
                .AddDistributedWorkflow(new DistributedWorkflowOptions
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
                .AddDistributedWorkflow(new DistributedWorkflowOptions
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
                .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
                .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
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
            .AddDistributedWorkflow(new DistributedWorkflowOptions
            {
                OutputPath = outputPath,
                PipelineProjectPath = new FilePath(@"src\My Pipeline's\Pipeline.csproj"),
            })
            .RunAsync();

        var yaml = (await outputPath.ReadAsync()).ReplaceLineEndings("\n");

        await Assert.That(yaml).Contains(
            "dotnet publish 'src/My Pipeline'\"'\"'s/Pipeline.csproj' -c Release");
        await Assert.That(yaml).Contains(
            "dotnet restore 'src/My Pipeline'\"'\"'s/Pipeline.csproj'");
        await Assert.That(yaml).Contains(
            "dotnet run --project 'src/My Pipeline'\"'\"'s/Pipeline.csproj' -c Release --no-build --no-restore");
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
