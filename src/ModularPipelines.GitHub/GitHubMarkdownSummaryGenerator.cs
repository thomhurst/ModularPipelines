using System.Text;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.GitHub;

internal class GitHubMarkdownSummaryGenerator : IPipelineGlobalHooks
{
    private const long MaxFileSizeInBytes = 1 * 1024 * 1024; // 1MB

    private readonly ISummaryLogger _summaryLogger;
    private readonly IDependencyGraphExporter _dependencyGraphExporter;

    public GitHubMarkdownSummaryGenerator(
        ISummaryLogger summaryLogger,
        IDependencyGraphExporter dependencyGraphExporter)
    {
        _summaryLogger = summaryLogger;
        _dependencyGraphExporter = dependencyGraphExporter;
    }

    public Task OnPipelineStartAsync(IPipelineContext pipelineContext)
    {
        return Task.CompletedTask;
    }

    public async Task OnPipelineEndAsync(
        IPipelineContext pipelineContext,
        PipelineSummary pipelineSummary)
    {
        var stepSummaryVariable = pipelineContext.Environment.Variables
            .Get("GITHUB_STEP_SUMMARY");
        if (string.IsNullOrEmpty(stepSummaryVariable))
        {
            return;
        }

        var mermaid = GenerateMermaidSummary(pipelineSummary);
        var dependencyGraph = await GenerateDependencyGraphAsync(pipelineSummary).ConfigureAwait(false);
        var table = GenerateTableSummary(pipelineSummary);
        var exception = GetException(pipelineSummary);

        await WriteFile(
            pipelineContext,
            stepSummaryVariable,
            dependencyGraph,
            mermaid,
            table,
            exception);
    }

    private async Task WriteFile(
        IPipelineContext pipelineContext,
        string stepSummaryVariable,
        string dependencyGraph,
        string mermaid,
        string table,
        string exception)
    {
        var fileInfo = pipelineContext.Files.GetFile(stepSummaryVariable);
        var currentFileSize = fileInfo.Exists ? fileInfo.Length : 0;
        var existingSummary = $"{mermaid}\n\n{table}\n\n{_summaryLogger.GetOutput()}{exception}";
        var contents = SelectContentsToAppend(currentFileSize, dependencyGraph, existingSummary);

        if (contents is null)
        {
            System.Console.WriteLine("Appending to the GitHub Step Summary would exceed the 1MB file size limit.");
            return;
        }

        await pipelineContext.Files.GetFile(stepSummaryVariable).AppendAsync(contents);
    }

    private static string? SelectContentsToAppend(
        long currentFileSize,
        string dependencyGraph,
        string existingSummary)
    {
        if (currentFileSize + Encoding.UTF8.GetByteCount(existingSummary) > MaxFileSizeInBytes)
        {
            return null;
        }

        var contentsWithGraph = $"{dependencyGraph}\n\n{existingSummary}";
        return currentFileSize + Encoding.UTF8.GetByteCount(contentsWithGraph) <= MaxFileSizeInBytes
            ? contentsWithGraph
            : existingSummary;
    }

    private async Task<string> GenerateDependencyGraphAsync(PipelineSummary pipelineSummary)
    {
        var graph = await _dependencyGraphExporter
            .RenderSummaryAsync(DependencyGraphFormat.Mermaid, pipelineSummary)
            .ConfigureAwait(false);
        return $"""
               ### Dependency Graph
               ```mermaid
               {graph}
               ```
               """;
    }

    private static string GetException(PipelineSummary pipelineSummary)
    {
        var exception = pipelineSummary.Results
                            .FirstOrDefault(x => x.Status == ModuleStatus.Failed)
                            ?.ExceptionOrDefault
                        ?? pipelineSummary.Results.Select(x => x.ExceptionOrDefault).FirstOrDefault();

        if (exception is null)
        {
            return string.Empty;
        }

        return $"\n\n```\n{exception}\n```";
    }

    private static string GenerateMermaidSummary(PipelineSummary pipelineSummary)
    {
        var stepStringList = pipelineSummary.Results
            .Where(x => x.Duration != TimeSpan.Zero)
            .OrderBy(x => x.StartTime)
            .ThenBy(s => s.EndTime)
            .Select(x => $"{x.Name} :{AddCritIfFailed(x)} {x.StartTime:HH:mm:ss:fff}, {x.EndTime:HH:mm:ss:fff}").ToList();

        var text = $"""
                    ```mermaid
                    ---
                    config:
                      theme: base
                      themeVariables:
                        primaryColor: "#2E7D32"
                        primaryTextColor: "#fff"
                        primaryBorderColor: "#558B2F"
                        lineColor: "#FF8F00"
                        secondaryColor: "#1B5E20"
                        tertiaryColor: "#fff"
                        darkmode: "true"
                        titleColor: "#fff"
                      gantt:
                        leftPadding: 40
                        rightPadding: 120
                    ---

                    gantt
                    	dateFormat  HH:mm:ss:SSS
                    	title       Run Summary
                    	axisFormat %H:%M:%S

                    {string.Join("\n", stepStringList)}
                    ```
                    """;

        return text;
    }

    private static string GenerateTableSummary(PipelineSummary pipelineSummary)
    {
        var stepStringList = pipelineSummary.Results.OrderBy(x => x.EndTime)
            .ThenBy(s => s.StartTime)
            .Select(module =>
                {
                    var isSameDay = module.StartTime.Date == module.EndTime.Date;

                    var (startTime, endTime, duration) = (module.StartTime, module.EndTime, module.Duration);
                    var text = $"| {module.Name} | {GetStatusString(module.Status)} | {GetTime(startTime, isSameDay)} | {GetTime(endTime, isSameDay)} | {duration} |";
                    return text;
                }
            ).ToList();

        var isSameDay = pipelineSummary.Start.Date == pipelineSummary.End.Date;
        var (globalStartTime, globalEndTime, globalDuration) = (pipelineSummary.Start, pipelineSummary.End, pipelineSummary.TotalDuration);
        var pipelineStatusString = GetStatusString(pipelineSummary.Status);
        var overallSummaryString = $"| **Total** | **{pipelineStatusString}** | **{GetTime(globalStartTime, isSameDay)}** | **{GetTime(globalEndTime, isSameDay)}** | **{globalDuration}** |";
        var text = $"""
                    ### Run Summary
                    | Step | Status | Start | End | Duration |
                    | --- | --- | --- | --- | --- |
                    {string.Join("\n", stepStringList)}
                    {overallSummaryString}
                    """;

        return text;
    }

    private static string AddCritIfFailed(IModuleResult moduleResult)
    {
        return moduleResult.ExceptionOrDefault is not null
            ? "crit,"
            : string.Empty;
    }

    internal static string GetStatusString(ModuleStatus status)
    {
        return status switch
        {
            ModuleStatus.Succeeded or ModuleStatus.RestoredFromHistory or ModuleStatus.RestoredFromCache =>
                $$$"""${\textsf{\color{lightgreen}{{{status}}}}}$""",
            ModuleStatus.NotStarted or ModuleStatus.FailureIgnored or ModuleStatus.Running or ModuleStatus.Skipped =>
                $$$"""${\textsf{\color{orange}{{{status}}}}}$""",
            ModuleStatus.Cancelled or ModuleStatus.TimedOut or ModuleStatus.Failed or ModuleStatus.DependencyFailed or ModuleStatus.Unknown =>
                $$$"""${\textsf{\color{red}{{{status}}}}}$""",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null),
        };
    }

    private static string GetTime(DateTimeOffset dateTimeOffset, bool isSameDay)
    {
        if (dateTimeOffset == DateTimeOffset.MinValue)
        {
            return string.Empty;
        }

        return isSameDay
            ? dateTimeOffset.ToString("h:mm:ss tt")
            : dateTimeOffset.ToString("yyyy/MM/dd h:mm:ss tt");
    }
}
