using ModularPipelines.Reporting;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;
using ModularPipelines.Enums;

namespace ModularPipelines.UnitTests.Helpers;

[TUnit.Core.NotInParallel]
public class SpectreResultsPrinterTests
{
    private class SkippedModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class FirstModule : SkippedModule
    {
    }

    private sealed class SecondModule : SkippedModule
    {
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
        await Assert.That(output).Contains("Status");
        await Assert.That(output).DoesNotContain("ModuleStatus");
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
    public async Task SummaryLine_SeparatesPendingModulesFromFailures()
    {
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary(
            [],
            [],
            TimeSpan.FromMinutes(18),
            now,
            now.AddMinutes(18),
            new PipelineMetrics
            {
                TotalModules = 68,
                SuccessfulModules = 34,
                FailedModules = 6,
                SkippedModules = 5,
                PendingModules = 23,
            });

        var summaryLine = SpectreResultsPrinter.CreateSummaryLine(summary);

        using (Assert.Multiple())
        {
            await Assert.That(summaryLine).Contains("34 passed");
            await Assert.That(summaryLine).Contains("6 failed");
            await Assert.That(summaryLine).Contains("5 skipped");
            await Assert.That(summaryLine).Contains("23 pending");
            await Assert.That(summaryLine).DoesNotContain("29 failed");
        }
    }

    [Test]
    public async Task SummaryLine_CountsDuplicateModuleTimelinesOnceEach()
    {
        var now = DateTimeOffset.UtcNow;
        var summary = new PipelineSummary(
            [new SkippedModule(), new SkippedModule()],
            [],
            TimeSpan.Zero,
            now,
            now,
            moduleTimelines:
            [
                new ModuleTimeline
                {
                    ModuleName = nameof(SkippedModule),
                    Status = ModuleStatus.Failed,
                },
                new ModuleTimeline
                {
                    ModuleName = nameof(SkippedModule),
                    Status = ModuleStatus.Skipped,
                    WasSkipped = true,
                },
            ]);

        var summaryLine = SpectreResultsPrinter.CreateSummaryLine(summary);

        using (Assert.Multiple())
        {
            await Assert.That(summaryLine).Contains("1 failed");
            await Assert.That(summaryLine).Contains("1 skipped");
        }
    }

    [Test]
    public async Task DistributedSummaryLine_ShowsIdlePercentageAndLongestQueueWait()
    {
        var line = SpectreResultsPrinter.CreateDistributedSummaryLine(new DistributedRunReport
        {
            FleetUtilizationPercentage = 60,
            Modules =
            [
                new DistributedModuleRunReport
                {
                    ModuleTypeName = "Example.FastModule, Example.Assembly",
                    QueueWaitDuration = TimeSpan.FromSeconds(1),
                },
                new DistributedModuleRunReport
                {
                    ModuleTypeName = "Example.SlowModule, Example.Assembly",
                    QueueWaitDuration = TimeSpan.FromSeconds(5),
                },
            ],
        });

        using (Assert.Multiple())
        {
            await Assert.That(line).Contains("Workers idle 40.0% of run");
            await Assert.That(line).Contains("longest queue wait: 5s 0ms on SlowModule");
        }
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
                    Status = ModuleStatus.Succeeded,
                    ExecutionDuration = TimeSpan.FromSeconds(5),
                },
            ]) with
        {
            RunReport = new PipelineRunReport
            {
                PreviousEnd = new DateTimeOffset(2026, 7, 27, 11, 58, 0, TimeSpan.Zero),
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
        await Assert.That(output).Contains("Δ vs run finished 2026-07-27 11:58 UTC");
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
    public async Task ModulesTable_MatchesDurationDeltasByModuleType()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var firstModule = new FirstModule();
        var secondModule = new SecondModule();
        var summary = new PipelineSummary(
            [firstModule, secondModule],
            [],
            TimeSpan.FromSeconds(5),
            start,
            start.AddSeconds(5)) with
        {
            RunReport = new PipelineRunReport
            {
                Modules =
                [
                    new ModuleRunReport
                    {
                        ModuleName = nameof(SecondModule),
                        ModuleTypeName = ModuleTypeIdentifier.Get(typeof(SecondModule)),
                        DurationDelta = TimeSpan.FromSeconds(2),
                    },
                    new ModuleRunReport
                    {
                        ModuleName = nameof(FirstModule),
                        ModuleTypeName = ModuleTypeIdentifier.Get(typeof(FirstModule)),
                        DurationDelta = TimeSpan.FromSeconds(1),
                    },
                ],
            },
        };

        var output = RenderToString(SpectreResultsPrinter.CreateModulesTable(summary));
        var lines = output.Split(Environment.NewLine);
        var firstModuleLine = lines.Single(line => line.Contains(nameof(FirstModule), StringComparison.Ordinal));
        var secondModuleLine = lines.Single(line => line.Contains(nameof(SecondModule), StringComparison.Ordinal));

        using (Assert.Multiple())
        {
            await Assert.That(firstModuleLine).Contains("+1s");
            await Assert.That(secondModuleLine).Contains("+2s");
        }
    }

    [Test]
    public async Task ResultsTable_IsVisibleWithoutBuildSystemGroup()
    {
        var output = PrintResults(CreateFailedSummary());

        var headline = output.IndexOf("Pipeline Failed", StringComparison.Ordinal);
        var counts = output.IndexOf("1 failed", StringComparison.Ordinal);
        var table = output.IndexOf(nameof(FailedModule), StringComparison.Ordinal);
        var failureDetails = output.IndexOf("Failed Modules", StringComparison.Ordinal);
        var metrics = output.IndexOf("Speedup:", StringComparison.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(headline).IsGreaterThanOrEqualTo(0);
            await Assert.That(counts).IsGreaterThan(headline);
            await Assert.That(table).IsGreaterThan(counts);
            await Assert.That(failureDetails).IsGreaterThan(table);
            await Assert.That(metrics).IsGreaterThan(failureDetails);
            await Assert.That(output).DoesNotContain("::group::Module Results");
            await Assert.That(output).Contains("root failure");
        }
    }

    [Test]
    public async Task ModulesTable_DoesNotApplyAmbiguousTypeDeltas()
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var summary = new PipelineSummary(
            [new FirstModule(), new FirstModule()],
            [],
            TimeSpan.FromSeconds(5),
            start,
            start.AddSeconds(5)) with
        {
            RunReport = new PipelineRunReport
            {
                TotalDurationDelta = TimeSpan.FromSeconds(3),
                Modules =
                [
                    new ModuleRunReport
                    {
                        ModuleName = nameof(FirstModule),
                        ModuleTypeName = ModuleTypeIdentifier.Get(typeof(FirstModule)),
                        DurationDelta = TimeSpan.FromSeconds(1),
                    },
                    new ModuleRunReport
                    {
                        ModuleName = nameof(FirstModule),
                        ModuleTypeName = ModuleTypeIdentifier.Get(typeof(FirstModule)),
                        DurationDelta = TimeSpan.FromSeconds(2),
                    },
                ],
            },
        };

        var output = RenderToString(SpectreResultsPrinter.CreateModulesTable(summary));

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("Δ previous");
            await Assert.That(output).DoesNotContain("+1s");
            await Assert.That(output).DoesNotContain("+2s");
        }
    }

    [Test]
    public async Task LocalOutput_DoesNotAddModuleResultsGroup()
    {
        var output = PrintResults(CreateFailedSummary());

        using (Assert.Multiple())
        {
            await Assert.That(output).DoesNotContain("Module Results");
            await Assert.That(output).Contains("Pipeline Failed");
            await Assert.That(output).Contains(nameof(FailedModule));
            await Assert.That(output).Contains("Failed Modules");
            await Assert.That(output).Contains("Speedup:");
        }
    }

    [Test]
    [Arguments(1, 10, 4)]
    [Arguments(4, 4, 4)]
    [Arguments(4, 3, 4)]
    public async Task Metrics_AreOmittedWhenModulesDidNotOverlapOrSaveTime(
        int peakConcurrency,
        int totalSeconds,
        int wallClockSeconds)
    {
        var metrics = new PipelineMetrics
        {
            ParallelismFactor = (double) totalSeconds / wallClockSeconds,
            PeakConcurrency = peakConcurrency,
            TotalModuleExecutionTime = TimeSpan.FromSeconds(totalSeconds),
            WallClockDuration = TimeSpan.FromSeconds(wallClockSeconds),
        };

        await Assert.That(SpectreResultsPrinter.ShouldPrintMetrics(metrics)).IsFalse();
    }

    [Test]
    public async Task Metrics_ArePrintedWhenParallelModulesSavedTime()
    {
        var metrics = new PipelineMetrics
        {
            ParallelismFactor = 2.5,
            PeakConcurrency = 4,
            TotalModuleExecutionTime = TimeSpan.FromSeconds(10),
            WallClockDuration = TimeSpan.FromSeconds(4),
        };

        using (Assert.Multiple())
        {
            await Assert.That(SpectreResultsPrinter.ShouldPrintMetrics(metrics)).IsTrue();
            await Assert.That(SpectreResultsPrinter.ShouldPrintMetrics(null)).IsFalse();
        }
    }

    [Test]
    public async Task SequentialRun_DoesNotPrintMetrics()
    {
        var summary = CreateFailedSummary(new PipelineMetrics
        {
            ParallelismFactor = 0.9,
            PeakConcurrency = 1,
            TotalModuleExecutionTime = TimeSpan.FromSeconds(4),
            WallClockDuration = TimeSpan.FromSeconds(5),
        });

        var output = PrintResults(summary);

        using (Assert.Multiple())
        {
            await Assert.That(output).DoesNotContain("Speedup:");
            await Assert.That(output).DoesNotContain("Saved:");
        }
    }

    [Test]
    public async Task SummaryEntries_ArePrintedInResultsAndNotLoggedAgain()
    {
        var logger = new CollectingLogger();
        var summaryLogger = new SummaryLogger(logger);
        summaryLogger.KeyValue("Version", "Generated Version Number", "1.2.3");
        summaryLogger.Success("Deployed [prod]");

        var output = PrintResults(CreateFailedSummary(), summaryLogger);
        summaryLogger.WriteLogs();

        var table = output.IndexOf(nameof(FailedModule), StringComparison.Ordinal);
        var heading = output.IndexOf("Summary", table, StringComparison.Ordinal);
        var category = output.IndexOf("Version", heading, StringComparison.Ordinal);
        var entry = output.IndexOf("Generated Version Number: 1.2.3", category, StringComparison.Ordinal);

        using (Assert.Multiple())
        {
            await Assert.That(heading).IsGreaterThan(table);
            await Assert.That(category).IsGreaterThan(heading);
            await Assert.That(entry).IsGreaterThan(category);
            await Assert.That(output).Contains("✓ Deployed [prod]");
            await Assert.That(logger.Messages).IsEmpty();
        }
    }

    [Test]
    public async Task SummaryEntries_AddedAfterResultsAreStillLogged()
    {
        var logger = new CollectingLogger();
        var summaryLogger = new SummaryLogger(logger);
        summaryLogger.Information("before");

        PrintResults(CreateFailedSummary(), summaryLogger);
        summaryLogger.Information("after");
        summaryLogger.WriteLogs();

        await Assert.That(logger.Messages).IsEquivalentTo(["after"]);
    }

    [Test]
    public async Task SummaryEntries_AreLoggedWhenResultsAreNotPrinted()
    {
        var logger = new CollectingLogger();
        var summaryLogger = new SummaryLogger(logger);
        summaryLogger.KeyValue("Version", "Generated Version Number", "1.2.3");

        summaryLogger.WriteLogs();

        await Assert.That(logger.Messages).IsEquivalentTo(["[Version] Generated Version Number: 1.2.3"]);
    }

    private sealed class CollectingLogger : Microsoft.Extensions.Logging.ILogger<SummaryLogger>
    {
        public List<string> Messages { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(Microsoft.Extensions.Logging.LogLevel logLevel) => true;

        public void Log<TState>(
            Microsoft.Extensions.Logging.LogLevel logLevel,
            Microsoft.Extensions.Logging.EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) => Messages.Add(formatter(state, exception));
    }

    private static PipelineSummary CreateFailedSummary(PipelineMetrics? metrics = null)
    {
        var start = new DateTimeOffset(2026, 7, 27, 12, 0, 0, TimeSpan.Zero);
        var end = start.AddSeconds(5);
        var module = new FailedModule();
        var result = new ModuleResult.Failure(new InvalidOperationException("root failure"))
        {
            Name = nameof(FailedModule),
            TypeName = ModuleTypeIdentifier.Get(typeof(FailedModule)),
            Duration = end - start,
            StartTime = start,
            EndTime = end,
            Status = ModuleStatus.Failed,
        };

        return new PipelineSummary(
            [module],
            [result],
            end - start,
            start,
            end,
            metrics ?? new PipelineMetrics
            {
                ParallelismFactor = 1.6,
                PeakConcurrency = 2,
                TotalModuleExecutionTime = TimeSpan.FromSeconds(8),
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

    private static string PrintResults(PipelineSummary summary, SummaryLogger? summaryLogger = null)
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

            var printer = new SpectreResultsPrinter(
                Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
                summaryLogger ?? new SummaryLogger(NullLogger<SummaryLogger>.Instance));

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
