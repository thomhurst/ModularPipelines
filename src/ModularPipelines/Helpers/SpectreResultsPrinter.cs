using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
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

        var table = CreateModulesTable(pipelineSummary);

        System.Console.WriteLine();
        AnsiConsole.Write(table.Expand());

        // Print execution metrics if available
        PrintMetrics(pipelineSummary);

        System.Console.WriteLine();
    }

    private static Table CreateModulesTable(PipelineSummary pipelineSummary)
    {
        var table = new Table
        {
            Expand = true,
        };

        table.AddColumn("Module");
        table.AddColumn("Duration");
        table.AddColumn("Status");
        table.AddColumn("Start");
        table.AddColumn("End");

        // Create a lookup for module timelines by module name
        var timelineLookup = pipelineSummary.ModuleTimelines?
            .ToDictionary(t => t.ModuleName, t => t)
            ?? new Dictionary<string, ModuleTimeline>();

        foreach (var module in pipelineSummary.Modules)
        {
            AddModuleRow(table, module, timelineLookup);
            table.AddEmptyRow();
        }

        AddTotalRow(table, pipelineSummary);

        return table;
    }

    private static void AddModuleRow(
        Table table,
        object module,
        Dictionary<string, ModuleTimeline> timelineLookup)
    {
        var moduleName = module.GetType().Name;
        var hasTimeline = timelineLookup.TryGetValue(moduleName, out var timeline);

        var duration = hasTimeline && timeline!.ExecutionDuration.HasValue
            ? timeline.ExecutionDuration.Value.ToDisplayString()
            : "-";

        var status = hasTimeline
            ? timeline!.Status.ToDisplayString()
            : "-";

        var isSameDay = hasTimeline
            && timeline!.StartTime.HasValue
            && timeline.EndTime.HasValue
            && timeline.StartTime.Value.Date == timeline.EndTime.Value.Date;

        var start = hasTimeline && timeline!.StartTime.HasValue
            ? FormatTime(timeline.StartTime.Value, isSameDay)
            : "-";

        var end = hasTimeline && timeline!.EndTime.HasValue
            ? FormatTime(timeline.EndTime.Value, isSameDay)
            : "-";

        table.AddRow(
            $"[cyan]{moduleName}[/]",
            duration,
            status,
            start,
            end);
    }

    private static void AddTotalRow(Table table, PipelineSummary pipelineSummary)
    {
        var isSameDayTotal = pipelineSummary.Start.Date == pipelineSummary.End.Date;

        table.AddRow(
            "Total",
            pipelineSummary.TotalDuration.ToDisplayString(),
            pipelineSummary.Status.ToDisplayString(),
            FormatTime(pipelineSummary.Start, isSameDayTotal),
            FormatTime(pipelineSummary.End, isSameDayTotal));
    }

    private static void PrintMetrics(PipelineSummary pipelineSummary)
    {
        var metrics = pipelineSummary.Metrics;
        if (metrics == null)
        {
            return;
        }

        System.Console.WriteLine();
        AnsiConsole.MarkupLine("[bold underline]Execution Metrics[/]");

        var metricsTable = new Table
        {
            Border = TableBorder.Rounded,
            ShowHeaders = false,
        };

        metricsTable.AddColumn("Metric");
        metricsTable.AddColumn("Value");

        metricsTable.AddRow("[cyan]Parallelism Factor[/]", $"[bold]{metrics.ParallelismFactor:F2}x[/]");
        metricsTable.AddRow("[cyan]Peak Concurrency[/]", $"[bold]{metrics.PeakConcurrency}[/] modules");
        metricsTable.AddRow("[cyan]Avg Concurrency[/]", $"[bold]{metrics.AverageConcurrency:F2}[/] modules");
        metricsTable.AddRow("[cyan]Efficiency[/]", $"[bold]{metrics.Efficiency * 100:F0}%[/]");
        metricsTable.AddRow("[cyan]Sequential Time[/]", $"[dim]{metrics.TotalModuleExecutionTime.ToDisplayString()}[/]");
        metricsTable.AddRow("[cyan]Wall-Clock Time[/]", $"[dim]{metrics.WallClockDuration.ToDisplayString()}[/]");

        AnsiConsole.Write(metricsTable);
    }

    private static string FormatTime(DateTimeOffset dateTimeOffset, bool isSameDay)
    {
        if (dateTimeOffset == DateTimeOffset.MinValue)
        {
            return string.Empty;
        }

        return isSameDay
            ? dateTimeOffset.ToTimeOnly().ToString("h:mm:ss tt")
            : dateTimeOffset.ToString("yyyy/MM/dd h:mm:ss tt");
    }
}
