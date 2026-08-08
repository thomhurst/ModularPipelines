using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;
using ModuleStatus = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Helpers;

[TUnit.Core.NotInParallel(nameof(SpectreResultsPrinterTests))]
public class SpectreResultsPrinterTests
{
    private class SkippedModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private class FailedModule : Module<bool>
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

    [Test]
    public async Task GitHubOutput_GroupsOnlyModuleResultsTable()
    {
        var output = PrintResults(CreateFailedSummary(), new GitHubActionsFormatter());

        var headline = output.IndexOf("Pipeline Failed", StringComparison.Ordinal);
        var counts = output.IndexOf("1 failed", StringComparison.Ordinal);
        var groupStart = output.IndexOf("::group::Module Results", StringComparison.Ordinal);
        var table = output.IndexOf(nameof(FailedModule), groupStart, StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", groupStart, StringComparison.Ordinal);
        var failureDetails = output.IndexOf("Failed Modules", StringComparison.Ordinal);
        var metrics = output.IndexOf("Speedup:", StringComparison.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(headline).IsGreaterThanOrEqualTo(0);
            await Assert.That(counts).IsGreaterThan(headline);
            await Assert.That(groupStart).IsGreaterThan(counts);
            await Assert.That(table).IsGreaterThan(groupStart);
            await Assert.That(groupEnd).IsGreaterThan(table);
            await Assert.That(failureDetails).IsGreaterThan(groupEnd);
            await Assert.That(metrics).IsGreaterThan(failureDetails);
            await Assert.That(output).Contains("root failure");
        }
    }

    [Test]
    public async Task LocalOutput_DoesNotAddModuleResultsGroup()
    {
        var output = PrintResults(CreateFailedSummary(), new DefaultFormatter());

        using (Assert.Multiple())
        {
            await Assert.That(output).DoesNotContain("Module Results");
            await Assert.That(output).Contains("Pipeline Failed");
            await Assert.That(output).Contains(nameof(FailedModule));
            await Assert.That(output).Contains("Failed Modules");
            await Assert.That(output).Contains("Speedup:");
        }
    }

    private static PipelineSummary CreateFailedSummary()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var end = start.AddSeconds(5);
        var module = new FailedModule();
        var result = new ModuleResult.Failure(new InvalidOperationException("root failure"))
        {
            ModuleName = nameof(FailedModule),
            ModuleTypeName = ModuleTypeIdentifier.Get(typeof(FailedModule)),
            ModuleDuration = end - start,
            ModuleStart = start,
            ModuleEnd = end,
            ModuleStatus = ModuleStatus.Failed,
        };

        return new PipelineSummary(
            [module],
            [result],
            end - start,
            start,
            end,
            new PipelineMetrics
            {
                ParallelismFactor = 1,
                PeakConcurrency = 1,
                TotalModuleExecutionTime = end - start,
                WallClockDuration = end - start,
                TotalModules = 1,
                FailedModules = 1,
            },
            [
                new ModuleTimeline
                {
                    ModuleName = nameof(FailedModule),
                    ModuleTypeName = ModuleTypeIdentifier.Get(typeof(FailedModule)),
                    StartTime = start,
                    EndTime = end,
                    ExecutionDuration = end - start,
                    Status = ModuleStatus.Failed,
                },
            ]);
    }

    private static string PrintResults(
        PipelineSummary summary,
        IBuildSystemFormatter formatter)
    {
        using var writer = new StringWriter();
        var originalAnsiConsole = AnsiConsole.Console;

        try
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(writer),
                Ansi = AnsiSupport.No,
                ColorSystem = ColorSystemSupport.NoColors,
            });

            var formatterProvider = new Mock<IBuildSystemFormatterProvider>();
            formatterProvider.Setup(x => x.GetFormatter()).Returns(formatter);
            var printer = new SpectreResultsPrinter(
                Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
                new BuildSystemCommandWriter(writer),
                formatterProvider.Object);

            printer.PrintResults(summary);
            return writer.ToString();
        }
        finally
        {
            AnsiConsole.Console = originalAnsiConsole;
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
