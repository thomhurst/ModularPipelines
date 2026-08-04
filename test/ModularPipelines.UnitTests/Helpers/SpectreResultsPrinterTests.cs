using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;
using ModuleStatus = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Helpers;

public class SpectreResultsPrinterTests
{
    private class SkippedModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [Test]
    public async Task ModulesTable_IsCompactWithoutBlankSeparator_AndLabelsSkippedModules()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var end = start.AddSeconds(5);
        var module = new SkippedModule();
        var metricsCollector = new Mock<IMetricsCollector>();
        metricsCollector
            .Setup(x => x.GetTimelines())
            .Returns(
            [
                new ModuleTimeline
                {
                    ModuleName = nameof(SkippedModule),
                    ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SkippedModule)),
                    Status = ModuleStatus.Skipped,
                    WasSkipped = true,
                },
            ]);

        var summary = new PipelineSummary(
            [module],
            [],
            end - start,
            start,
            end,
            metricsCollector.Object.ComputeMetrics(start, end, maxParallelism: 1),
            metricsCollector.Object.GetTimelines());

        var table = SpectreResultsPrinter.CreateModulesTable(summary);
        var output = RenderToString(table);

        await Assert.That(table.Expand).IsFalse();
        await Assert.That(table.Rows.Count).IsEqualTo(2);
        await Assert.That(output).Contains("⏭ skipped");
    }

    [Test]
    public async Task MetricsPanel_LabelsParallelismFactorAsSpeedup()
    {
        var panel = SpectreResultsPrinter.CreateMetricsPanel(new PipelineMetrics
        {
            ParallelismFactor = 2.5,
            PeakConcurrency = 4,
            TotalModuleExecutionTime = TimeSpan.FromSeconds(10),
            WallClockDuration = TimeSpan.FromSeconds(4),
        });

        var output = RenderToString(panel);

        await Assert.That(output).Contains("Speedup:");
        await Assert.That(output).DoesNotContain("Parallelism:");
        await Assert.That(output).Contains("2.5x");
    }

    [Test]
    public async Task ModulesTable_ShowsPreviousRunDurationDelta()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var module = new SkippedModule();
        var summary = new PipelineSummary(
            [module],
            [],
            TimeSpan.FromSeconds(5),
            start,
            start.AddSeconds(5),
            moduleTimelines:
            [
                new ModuleTimeline
                {
                    ModuleName = nameof(SkippedModule),
                    ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SkippedModule)),
                    Status = ModuleStatus.Successful,
                    ExecutionDuration = TimeSpan.FromSeconds(5),
                },
            ]) with
        {
            RunReport = new PipelineRunReport
            {
                TotalDurationDelta = TimeSpan.FromSeconds(2),
                Modules =
                [
                    new ModuleRunReport
                    {
                        ModuleName = nameof(SkippedModule),
                        ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SkippedModule)),
                        DurationDelta = TimeSpan.FromSeconds(2),
                    },
                ],
            },
        };

        var output = RenderToString(SpectreResultsPrinter.CreateModulesTable(summary));

        await Assert.That(output).Contains("Δ previous");
        await Assert.That(output).Contains("+2s");
    }

    [Test]
    public async Task ModulesTable_ShowsTotalDeltaWithoutMatchingModuleDeltas()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var module = new SkippedModule();
        var summary = new PipelineSummary(
            [module],
            [],
            TimeSpan.FromSeconds(5),
            start,
            start.AddSeconds(5)) with
        {
            RunReport = new PipelineRunReport
            {
                TotalDurationDelta = TimeSpan.FromSeconds(2),
                Modules = [],
            },
        };

        var output = RenderToString(SpectreResultsPrinter.CreateModulesTable(summary));

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("Δ previous");
            await Assert.That(output).Contains("+2s");
        }
    }

    private static string RenderToString(IRenderable renderable)
    {
        using var writer = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(writer),
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
        });

        console.Write(renderable);
        return writer.ToString();
    }
}
