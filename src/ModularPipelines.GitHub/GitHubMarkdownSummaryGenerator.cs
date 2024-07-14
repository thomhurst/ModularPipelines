using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.GitHub;

internal class GitHubMarkdownSummaryGenerator : IPipelineGlobalHooks
{
    private readonly IAfterPipelineLogger _afterPipelineLogger;

    public GitHubMarkdownSummaryGenerator(IAfterPipelineLogger afterPipelineLogger)
    {
        _afterPipelineLogger = afterPipelineLogger;
    }
    
    public Task OnStartAsync(IPipelineHookContext pipelineContext)
    {
        return Task.CompletedTask;
    }

    public async Task OnEndAsync(IPipelineHookContext pipelineContext, PipelineSummary pipelineSummary)
    {
        var mermaid = await GenerateMermaidSummary(pipelineSummary);
        var table = await GenerateTableSummary(pipelineSummary);

        var stepSummaryVariable = pipelineContext.Environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_STEP_SUMMARY");
        
        if (string.IsNullOrEmpty(stepSummaryVariable))
        {
            return;
        }

        await pipelineContext.FileSystem.GetFile(stepSummaryVariable).WriteAsync($"{mermaid}\n\n{table}\n\n{_afterPipelineLogger.GetOutput()}");
    }

    private async Task<string> GenerateMermaidSummary(PipelineSummary pipelineSummary)
    {
        var results = await pipelineSummary.GetModuleResultsAsync();

        var stepStringList = results
            .Where(x => x.ModuleDuration != TimeSpan.Zero)
            .OrderBy(x => x.ModuleStart)
            .ThenBy(s => s.ModuleEnd)
            .Select(x => $"{x.ModuleName} :{AddCritIfFailed(x)} {x.ModuleStart:HH:mm:ss:fff}, {x.ModuleEnd:HH:mm:ss:fff}").ToList();

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

    private async Task<string> GenerateTableSummary(PipelineSummary pipelineSummary)
    {
        var results = await pipelineSummary.GetModuleResultsAsync();
        
        var stepStringList = results.OrderBy(x => x.ModuleEnd)
            .ThenBy(s => s.ModuleStart)
            .Select(module =>
                {
                    var isSameDay = module.ModuleStart.Date == module.ModuleEnd.Date;

                    var (startTime, endTime, duration) = (module.ModuleStart, module.ModuleEnd, module.ModuleDuration);
                    var text = $"| {module.ModuleName} | {GetStatusString(module.ModuleStatus)} | {GetTime(startTime, isSameDay)} | {GetTime(endTime, isSameDay)} | {duration} |";
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
        return moduleResult.ModuleResultType is ModuleResultType.Failure
            ? "crit,"
            : string.Empty;
    }
    
    private static string GetStatusString(Status status)
    {
        return status switch
        {
            Status.Successful or Status.UsedHistory => $$$"""${\textsf{\color{lightgreen}{{{status}}}}}$""",
            Status.NotYetStarted or Status.IgnoredFailure or Status.Processing or Status.Skipped =>
                $$$"""${\textsf{\color{orange}{{{status}}}}}$""",
            Status.PipelineTerminated or Status.TimedOut or Status.Failed or Status.Unknown =>
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