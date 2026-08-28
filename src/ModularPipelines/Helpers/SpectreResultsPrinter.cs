using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Spectre.Console;

namespace ModularPipelines.Helpers;

/// <summary>
/// Spectre.Console implementation of results printing.
/// Handles all console rendering for pipeline execution results.
/// </summary>
[ExcludeFromCodeCoverage]
internal class SpectreResultsPrinter : IResultsPrinter
{
    private const int MaxStackFrames = 5;

    private readonly IOptions<PipelineOptions> _options;

    public SpectreResultsPrinter(IOptions<PipelineOptions> options)
    {
        _options = options;
    }

    public void PrintResults(PipelineSummary pipelineSummary)
    {
        if (!_options.Value.Console.PrintResults)
        {
            return;
        }

        System.Console.WriteLine();

        // Print header with summary counts
        PrintHeader(pipelineSummary);

        // Create and print the main results table
        var table = CreateModulesTable(pipelineSummary);
        AnsiConsole.Write(table);

        // Print failed module details if pipeline failed
        if (pipelineSummary.Status == ModuleStatus.Failed)
        {
            PrintFailedModules(pipelineSummary);
        }

        // Print execution metrics if available
        PrintMetrics(pipelineSummary);

        System.Console.WriteLine();
    }

    internal static Table CreateModulesTable(PipelineSummary pipelineSummary) =>
        CreateModulesTableCore(pipelineSummary);

    internal static Panel CreateMetricsPanel(PipelineMetrics metrics) =>
        CreateMetricsPanelCore(metrics);

    internal static string CreateSummaryLine(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;

        var successCount = metrics?.SuccessfulModules
            ?? CountModules(pipelineSummary, static timeline => timeline.WasSuccessful);
        var failedCount = metrics?.FailedModules
            ?? CountModules(pipelineSummary, static timeline => timeline.Status is ModuleStatus.Failed
                or ModuleStatus.TimedOut
                or ModuleStatus.Cancelled
                or ModuleStatus.DependencyFailed);
        var skippedCount = metrics?.SkippedModules
            ?? CountModules(pipelineSummary, static timeline => timeline.WasSkipped);
        var ignoredCount = metrics?.IgnoredFailureModules
            ?? CountModules(pipelineSummary, static timeline => timeline.Status == ModuleStatus.FailureIgnored);
        var pendingCount = metrics?.PendingModules
            ?? CountModules(pipelineSummary, static timeline => timeline.Status == ModuleStatus.NotStarted);
        var processingCount = metrics?.ProcessingModules
            ?? CountModules(pipelineSummary, static timeline => timeline.Status == ModuleStatus.Running);
        var unknownCount = metrics?.UnknownModules
            ?? CountModules(pipelineSummary, static timeline => timeline.Status == ModuleStatus.Unknown);

        var parts = new List<string>();

        AddSummaryCount(parts, successCount, "green", "passed");
        AddSummaryCount(parts, failedCount, "red", "failed");
        AddSummaryCount(parts, ignoredCount, "yellow", "ignored");
        AddSummaryCount(parts, skippedCount, "yellow", "skipped");
        AddSummaryCount(parts, pendingCount, "grey", "pending");
        AddSummaryCount(parts, processingCount, "blue", "running");
        AddSummaryCount(parts, unknownCount, "grey", "unknown");

        var totalCount = metrics?.TotalModules ?? pipelineSummary.Modules.Count;
        return parts.Count > 0
            ? string.Join("[dim] | [/]", parts)
            : $"[dim]{totalCount} modules[/]";
    }

    private static void PrintHeader(PipelineSummary pipelineSummary)
    {
        var summaryLine = CreateSummaryLine(pipelineSummary);

        // Create the header rule
        var headerText = pipelineSummary.Status == ModuleStatus.Succeeded
            ? "[bold green]Pipeline Completed Successfully[/]"
            : pipelineSummary.Status == ModuleStatus.Failed
                ? "[bold red]Pipeline Failed[/]"
                : $"[bold]Pipeline {pipelineSummary.Status}[/]";

        AnsiConsole.MarkupLine(headerText);
        AnsiConsole.MarkupLine($"[dim]Duration:[/] [bold]{pipelineSummary.TotalDuration.ToDisplayString()}[/]  {summaryLine}");
        System.Console.WriteLine();
    }

    private static void AddSummaryCount(List<string> parts, int count, string color, string label)
    {
        if (count > 0)
        {
            parts.Add($"[{color}]{count} {label}[/]");
        }
    }

    private static int CountModules(PipelineSummary pipelineSummary, Func<ModuleTimeline, bool> predicate)
    {
        return pipelineSummary.ModuleTimelines?.Count(predicate) ?? 0;
    }

    private static Table CreateModulesTableCore(PipelineSummary pipelineSummary)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
        };

        // Add columns with alignment
        table.AddColumn(new TableColumn("[bold]Module[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Duration[/]").RightAligned());
        var reportLookup = pipelineSummary.RunReport is null
            ? new Dictionary<string, ModuleRunReport>(StringComparer.Ordinal)
            : pipelineSummary.RunReport.Modules
                .ToUniqueByKeyDictionary(static report => report.ModuleTypeName);
        var showDeltas = pipelineSummary.RunReport?.TotalDurationDelta.HasValue == true
            || reportLookup.Values.Any(static module => module.DurationDelta.HasValue);
        if (showDeltas)
        {
            table.AddColumn(new TableColumn("[bold]Δ previous[/]").RightAligned());
        }

        table.AddColumn(new TableColumn("[bold]Start[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]End[/]").RightAligned());

        // Create a lookup for module timelines by assembly-qualified module type
        var timelineLookup = pipelineSummary.ModuleTimelines?
            .ToFirstByKeyDictionary(
                static timeline => string.IsNullOrWhiteSpace(timeline.RuntimeModuleTypeName)
                    ? timeline.ModuleTypeName
                    : timeline.RuntimeModuleTypeName)
            ?? new Dictionary<string, ModuleTimeline>(StringComparer.Ordinal);

        // Sort modules: Failed first, then Skipped, then by start time
        var sortedModules = pipelineSummary.Modules
            .OrderBy(m =>
            {
                if (TryGetTimeline(timelineLookup, m.GetType(), out var timeline))
                {
                    return timeline.Status switch
                    {
                        ModuleStatus.Failed => 0,
                        ModuleStatus.TimedOut => 0,
                        ModuleStatus.Cancelled => 0,
                        ModuleStatus.DependencyFailed => 0,
                        ModuleStatus.FailureIgnored => 1,
                        ModuleStatus.Skipped => 2,
                        _ => 3,
                    };
                }

                return 3;
            })
            .ThenBy(m =>
            {
                return TryGetTimeline(timelineLookup, m.GetType(), out var timeline)
                    ? timeline.StartTime ?? DateTimeOffset.MaxValue
                    : DateTimeOffset.MaxValue;
            })
            .ToList();

        foreach (var module in sortedModules)
        {
            AddModuleRow(table, module, timelineLookup, reportLookup, showDeltas);
        }

        AddTotalRow(table, pipelineSummary, showDeltas);

        if (showDeltas && pipelineSummary.RunReport?.PreviousEnd is { } previousEnd)
        {
            var baseline = previousEnd.ToUniversalTime()
                .ToString("yyyy-MM-dd HH:mm 'UTC'", CultureInfo.InvariantCulture);
            table.Caption($"[dim]Δ vs run finished {baseline}[/]");
        }

        return table;
    }

    private static void AddModuleRow(
        Table table,
        object module,
        Dictionary<string, ModuleTimeline> timelineLookup,
        IReadOnlyDictionary<string, ModuleRunReport> reportLookup,
        bool showDeltas)
    {
        var moduleName = module.GetType().Name;
        var escapedModuleName = SpectreMarkupEscaper.Escape(moduleName);
        var hasTimeline = TryGetTimeline(timelineLookup, module.GetType(), out var timeline);

        var duration = hasTimeline && timeline!.ExecutionDuration.HasValue
            ? $"[dim]{timeline.ExecutionDuration.Value.ToDisplayString()}[/]"
            : "[dim]-[/]";

        var status = hasTimeline
            ? FormatStatusWithIcon(timeline!.Status)
            : "[dim]-[/]";

        var isSameDay = hasTimeline
            && timeline!.StartTime.HasValue
            && timeline.EndTime.HasValue
            && timeline.StartTime.Value.Date == timeline.EndTime.Value.Date;

        var start = hasTimeline && timeline!.StartTime.HasValue
            ? $"[dim]{FormatTime(timeline.StartTime.Value, isSameDay)}[/]"
            : "[dim]-[/]";

        var end = hasTimeline && timeline!.EndTime.HasValue
            ? $"[dim]{FormatTime(timeline.EndTime.Value, isSameDay)}[/]"
            : "[dim]-[/]";

        // Color module name based on status, using escaped name to prevent markup errors
        var moduleNameFormatted = hasTimeline
            ? FormatModuleNameByStatus(escapedModuleName, timeline!.Status)
            : $"[cyan]{escapedModuleName}[/]";

        var cells = new List<string>
        {
            moduleNameFormatted,
            status,
            duration,
        };
        if (showDeltas)
        {
            cells.Add(reportLookup.TryGetValue(ModuleTypeIdentifier.Get(module.GetType()), out var report)
                ? FormatDelta(report.DurationDelta)
                : "[dim]-[/]");
        }

        cells.AddRange(
        [
            start,
            end,
        ]);
        table.AddRow(cells.ToArray());
    }

    private static string FormatModuleNameByStatus(string moduleName, ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.Failed => $"[red]{moduleName}[/]",
            ModuleStatus.TimedOut => $"[red]{moduleName}[/]",
            ModuleStatus.Cancelled => $"[red]{moduleName}[/]",
            ModuleStatus.DependencyFailed => $"[red]{moduleName}[/]",
            ModuleStatus.FailureIgnored => $"[yellow]{moduleName}[/]",
            ModuleStatus.Skipped => $"[dim]{moduleName}[/]",
            ModuleStatus.Succeeded => $"[green]{moduleName}[/]",
            ModuleStatus.RestoredFromHistory => $"[green3]{moduleName}[/]",
            ModuleStatus.RestoredFromCache => $"[green3]{moduleName}[/]",
            _ => $"[cyan]{moduleName}[/]",
        };
    }

    private static string FormatStatusWithIcon(ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.Succeeded => "[green]Passed[/]",
            ModuleStatus.Failed => "[red]Failed[/]",
            ModuleStatus.TimedOut => "[red]Timeout[/]",
            ModuleStatus.Cancelled => "[red]Terminated[/]",
            ModuleStatus.DependencyFailed => "[red]Dependency Failed[/]",
            ModuleStatus.FailureIgnored => "[yellow]Ignored[/]",
            ModuleStatus.Skipped => "[dim]⏭ skipped[/]",
            ModuleStatus.RestoredFromHistory => "[green3]History[/]",
            ModuleStatus.RestoredFromCache => "[green3]Cached[/]",
            ModuleStatus.Running => "[blue]Running[/]",
            ModuleStatus.NotStarted => "[dim]Pending[/]",
            ModuleStatus.Unknown => "[dim]Unknown[/]",
            _ => "[dim]-[/]",
        };
    }

    private static void AddTotalRow(
        Table table,
        PipelineSummary pipelineSummary,
        bool showDeltas)
    {
        var isSameDayTotal = pipelineSummary.Start.Date == pipelineSummary.End.Date;

        var statusFormatted = pipelineSummary.Status == ModuleStatus.Succeeded
            ? "[bold green]Passed[/]"
            : pipelineSummary.Status == ModuleStatus.Failed
                ? "[bold red]Failed[/]"
                : $"[bold]{pipelineSummary.Status}[/]";

        var cells = new List<string>
        {
            "[bold]Total[/]",
            statusFormatted,
            $"[bold]{pipelineSummary.TotalDuration.ToDisplayString()}[/]",
        };
        if (showDeltas)
        {
            cells.Add(FormatDelta(pipelineSummary.RunReport?.TotalDurationDelta));
        }

        cells.AddRange(
        [
            $"[dim]{FormatTime(pipelineSummary.Start, isSameDayTotal)}[/]",
            $"[dim]{FormatTime(pipelineSummary.End, isSameDayTotal)}[/]",
        ]);
        table.AddRow(cells.ToArray());
    }

    private static bool TryGetTimeline(
        IReadOnlyDictionary<string, ModuleTimeline> timelines,
        Type moduleType,
        out ModuleTimeline timeline) =>
        timelines.TryGetValue(ModuleTypeIdentifier.GetRuntime(moduleType), out timeline!)
        || timelines.TryGetValue(ModuleTypeIdentifier.Get(moduleType), out timeline!);

    private static string FormatDelta(TimeSpan? delta)
    {
        if (!delta.HasValue)
        {
            return "[dim]-[/]";
        }

        if (delta.Value == TimeSpan.Zero)
        {
            return "[dim]±0s[/]";
        }

        var sign = delta.Value > TimeSpan.Zero ? "+" : "-";
        var color = delta.Value > TimeSpan.Zero ? "yellow" : "green";
        return $"[{color}]{sign}{delta.Value.Duration().ToDisplayString()}[/]";
    }

    private static void PrintMetrics(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;
        if (metrics == null)
        {
            return;
        }

        System.Console.WriteLine();

        AnsiConsole.Write(CreateMetricsPanel(metrics));
    }

    private static Panel CreateMetricsPanelCore(PipelineMetrics metrics)
    {
        return new Panel(
            new Markup(
                $"[dim]Speedup:[/] [bold]{metrics.ParallelismFactor:F1}x[/]  " +
                $"[dim]Peak:[/] [bold]{metrics.PeakConcurrency}[/]  " +
                $"[dim]Saved:[/] [bold]{(metrics.TotalModuleExecutionTime - metrics.WallClockDuration).ToDisplayString()}[/]"))
        {
            Border = BoxBorder.None,
            Padding = new Padding(0, 0, 0, 0),
        };
    }

    private static string FormatTime(DateTimeOffset dateTimeOffset, bool isSameDay)
    {
        if (dateTimeOffset == DateTimeOffset.MinValue)
        {
            return "-";
        }

        return isSameDay
            ? dateTimeOffset.ToTimeOnly().ToString("HH:mm:ss")
            : dateTimeOffset.ToString("MM/dd HH:mm:ss");
    }

    private static void PrintFailedModules(PipelineSummary pipelineSummary)
    {
        // Only show modules that actually failed, not cascaded failures (Cancelled)
        // Cascaded failures are modules that never started because their dependencies failed
        var failedResults = pipelineSummary.Results
            .Where(result => result.ExceptionOrDefault is not null)
            .Where(r => r.Status is ModuleStatus.Failed or ModuleStatus.TimedOut)
            .ToList();

        if (failedResults.Count == 0)
        {
            return;
        }

        System.Console.WriteLine();
        AnsiConsole.MarkupLine($"{MarkupFormatter.WarningIcon} [bold red]Failed Modules[/]");
        System.Console.WriteLine();

        foreach (var result in failedResults)
        {
            var exception = result.ExceptionOrDefault;
            if (exception == null)
            {
                continue;
            }

            var escapedModuleName = SpectreMarkupEscaper.Escape(result.Name);
            AnsiConsole.MarkupLine($"  [red]\u2717[/] [bold]{escapedModuleName}[/]");

            PrintException(exception, isInner: false);

            // Print inner exceptions
            var innerException = exception.InnerException;
            while (innerException != null)
            {
                AnsiConsole.MarkupLine("    [dim]\u2500\u2500\u2500 Inner Exception \u2500\u2500\u2500[/]");
                PrintException(innerException, isInner: true);
                innerException = innerException.InnerException;
            }

            System.Console.WriteLine();
        }
    }

    private static void PrintException(Exception exception, bool isInner)
    {
        var exceptionTypeName = exception.GetType().Name;
        var escapedMessage = SpectreMarkupEscaper.Escape(exception.Message);

        AnsiConsole.MarkupLine($"    [yellow]{exceptionTypeName}[/]: {escapedMessage}");

        // Print first few stack frames if available
        if (!string.IsNullOrEmpty(exception.StackTrace))
        {
            var stackLines = exception.StackTrace
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Take(MaxStackFrames)
                .ToList();

            foreach (var line in stackLines)
            {
                var trimmedLine = line.TrimStart();
                var escapedLine = SpectreMarkupEscaper.Escape(trimmedLine);
                AnsiConsole.MarkupLine($"      [dim]{escapedLine}[/]");
            }

            // Indicate if there are more frames
            var totalFrames = exception.StackTrace
                .Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries)
                .Length;

            if (totalFrames > MaxStackFrames)
            {
                AnsiConsole.MarkupLine($"      [dim]... and {totalFrames - MaxStackFrames} more frames[/]");
            }
        }
    }
}
