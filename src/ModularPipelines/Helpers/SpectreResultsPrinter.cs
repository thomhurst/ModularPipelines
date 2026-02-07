using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using Spectre.Console;
using ModuleStatus = ModularPipelines.Enums.Status;

namespace ModularPipelines.Helpers;

/// <summary>
/// Spectre.Console implementation of results printing.
/// Handles all console rendering for pipeline execution results.
/// </summary>
[ExcludeFromCodeCoverage]
internal class SpectreResultsPrinter : IResultsPrinter
{
    private readonly IOptions<PipelineOptions> _options;

    public SpectreResultsPrinter(IOptions<PipelineOptions> options)
    {
        _options = options;
    }

    public void PrintResults(PipelineSummary pipelineSummary)
    {
        if (!_options.Value.PrintResults)
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

    private static void PrintHeader(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;

        // Build summary counts
        var successCount = metrics?.SuccessfulModules ?? pipelineSummary.Modules.Count(m =>
        {
            var timeline = pipelineSummary.ModuleTimelines?.FirstOrDefault(t => t.ModuleName == m.GetType().Name);
            return timeline?.WasSuccessful == true;
        });

        var failedCount = metrics?.FailedModules ?? pipelineSummary.Modules.Count(m =>
        {
            var timeline = pipelineSummary.ModuleTimelines?.FirstOrDefault(t => t.ModuleName == m.GetType().Name);
            return timeline?.Status == ModuleStatus.Failed || timeline?.Status == ModuleStatus.TimedOut;
        });

        var skippedCount = metrics?.SkippedModules ?? pipelineSummary.Modules.Count(m =>
        {
            var timeline = pipelineSummary.ModuleTimelines?.FirstOrDefault(t => t.ModuleName == m.GetType().Name);
            return timeline?.WasSkipped == true;
        });

        var ignoredCount = pipelineSummary.Modules.Count(m =>
        {
            var timeline = pipelineSummary.ModuleTimelines?.FirstOrDefault(t => t.ModuleName == m.GetType().Name);
            return timeline?.Status == ModuleStatus.IgnoredFailure;
        });

        var totalCount = metrics?.TotalModules ?? pipelineSummary.Modules.Count;

        // Build the summary line
        var parts = new List<string>();

        if (successCount > 0)
        {
            parts.Add($"[green]{successCount} passed[/]");
        }

        if (failedCount > 0)
        {
            parts.Add($"[red]{failedCount} failed[/]");
        }

        if (ignoredCount > 0)
        {
            parts.Add($"[yellow]{ignoredCount} ignored[/]");
        }

        if (skippedCount > 0)
        {
            parts.Add($"[yellow]{skippedCount} skipped[/]");
        }

        var summaryLine = parts.Count > 0
            ? string.Join("[dim] | [/]", parts)
            : $"[dim]{totalCount} modules[/]";

        // Create the header rule
        var headerText = pipelineSummary.Status == ModuleStatus.Successful
            ? "[bold green]Pipeline Completed Successfully[/]"
            : pipelineSummary.Status == ModuleStatus.Failed
                ? "[bold red]Pipeline Failed[/]"
                : $"[bold]Pipeline {pipelineSummary.Status}[/]";

        AnsiConsole.MarkupLine(headerText);
        AnsiConsole.MarkupLine($"[dim]Duration:[/] [bold]{pipelineSummary.TotalDuration.ToDisplayString()}[/]  {summaryLine}");
        System.Console.WriteLine();
    }

    private static Table CreateModulesTable(PipelineSummary pipelineSummary)
    {
        var table = new Table
        {
            Border = TableBorder.Rounded,
            Expand = true,
        };

        // Add columns with alignment
        table.AddColumn(new TableColumn("[bold]Module[/]").LeftAligned());
        table.AddColumn(new TableColumn("[bold]Status[/]").Centered());
        table.AddColumn(new TableColumn("[bold]Duration[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]Start[/]").RightAligned());
        table.AddColumn(new TableColumn("[bold]End[/]").RightAligned());

        // Create a lookup for module timelines by module name
        var timelineLookup = pipelineSummary.ModuleTimelines?
            .ToDictionary(t => t.ModuleName, t => t)
            ?? new Dictionary<string, ModuleTimeline>();

        // Sort modules: Failed first, then Skipped, then by start time
        var sortedModules = pipelineSummary.Modules
            .OrderBy(m =>
            {
                var name = m.GetType().Name;
                if (timelineLookup.TryGetValue(name, out var timeline))
                {
                    return timeline.Status switch
                    {
                        ModuleStatus.Failed => 0,
                        ModuleStatus.TimedOut => 0,
                        ModuleStatus.PipelineTerminated => 0,
                        ModuleStatus.IgnoredFailure => 1,
                        ModuleStatus.Skipped => 2,
                        _ => 3
                    };
                }

                return 3;
            })
            .ThenBy(m =>
            {
                var name = m.GetType().Name;
                return timelineLookup.TryGetValue(name, out var timeline)
                    ? timeline.StartTime ?? DateTimeOffset.MaxValue
                    : DateTimeOffset.MaxValue;
            })
            .ToList();

        foreach (var module in sortedModules)
        {
            AddModuleRow(table, module, timelineLookup);
        }

        // Add separator and total row
        table.AddEmptyRow();
        AddTotalRow(table, pipelineSummary);

        return table;
    }

    private static void AddModuleRow(
        Table table,
        object module,
        Dictionary<string, ModuleTimeline> timelineLookup)
    {
        var moduleName = module.GetType().Name;
        var escapedModuleName = SpectreMarkupEscaper.Escape(moduleName);
        var hasTimeline = timelineLookup.TryGetValue(moduleName, out var timeline);

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

        table.AddRow(
            moduleNameFormatted,
            status,
            duration,
            start,
            end);
    }

    private static string FormatModuleNameByStatus(string moduleName, ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.Failed => $"[red]{moduleName}[/]",
            ModuleStatus.TimedOut => $"[red]{moduleName}[/]",
            ModuleStatus.PipelineTerminated => $"[red]{moduleName}[/]",
            ModuleStatus.IgnoredFailure => $"[yellow]{moduleName}[/]",
            ModuleStatus.Skipped => $"[dim]{moduleName}[/]",
            ModuleStatus.Successful => $"[green]{moduleName}[/]",
            ModuleStatus.UsedHistory => $"[green3]{moduleName}[/]",
            _ => $"[cyan]{moduleName}[/]"
        };
    }

    private static string FormatStatusWithIcon(ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.Successful => "[green]Passed[/]",
            ModuleStatus.Failed => "[red]Failed[/]",
            ModuleStatus.TimedOut => "[red]Timeout[/]",
            ModuleStatus.PipelineTerminated => "[red]Terminated[/]",
            ModuleStatus.IgnoredFailure => "[yellow]Ignored[/]",
            ModuleStatus.Skipped => "[yellow]Skipped[/]",
            ModuleStatus.UsedHistory => "[green3]Cached[/]",
            ModuleStatus.Retried => "[yellow]Retried[/]",
            ModuleStatus.Processing => "[blue]Running[/]",
            ModuleStatus.NotYetStarted => "[dim]Pending[/]",
            ModuleStatus.Unknown => "[dim]Unknown[/]",
            _ => "[dim]-[/]"
        };
    }

    private static void AddTotalRow(Table table, PipelineSummary pipelineSummary)
    {
        var isSameDayTotal = pipelineSummary.Start.Date == pipelineSummary.End.Date;

        var statusFormatted = pipelineSummary.Status == ModuleStatus.Successful
            ? "[bold green]Passed[/]"
            : pipelineSummary.Status == ModuleStatus.Failed
                ? "[bold red]Failed[/]"
                : $"[bold]{pipelineSummary.Status}[/]";

        table.AddRow(
            "[bold]Total[/]",
            statusFormatted,
            $"[bold]{pipelineSummary.TotalDuration.ToDisplayString()}[/]",
            $"[dim]{FormatTime(pipelineSummary.Start, isSameDayTotal)}[/]",
            $"[dim]{FormatTime(pipelineSummary.End, isSameDayTotal)}[/]");
    }

    private static void PrintMetrics(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;
        if (metrics == null)
        {
            return;
        }

        System.Console.WriteLine();

        // Create a compact metrics display
        var metricsPanel = new Panel(
            new Markup(
                $"[dim]Parallelism:[/] [bold]{metrics.ParallelismFactor:F1}x[/]  " +
                $"[dim]Peak:[/] [bold]{metrics.PeakConcurrency}[/]  " +
                $"[dim]Saved:[/] [bold]{(metrics.TotalModuleExecutionTime - metrics.WallClockDuration).ToDisplayString()}[/]"))
        {
            Border = BoxBorder.None,
            Padding = new Padding(0, 0, 0, 0),
        };

        AnsiConsole.Write(metricsPanel);
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
        // Only show modules that actually failed, not cascaded failures (PipelineTerminated)
        // Cascaded failures are modules that never started because their dependencies failed
        var failedResults = pipelineSummary.GetFailedModuleResults()
            .Where(r => r.ModuleStatus is ModuleStatus.Failed or ModuleStatus.TimedOut)
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

            var escapedModuleName = SpectreMarkupEscaper.Escape(result.ModuleName);
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

    private const int MaxStackFrames = 5;
}
