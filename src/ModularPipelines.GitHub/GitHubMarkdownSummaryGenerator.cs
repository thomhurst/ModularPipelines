using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;

namespace ModularPipelines.GitHub;

internal class GitHubMarkdownSummaryGenerator : IPipelineGlobalHooks
{
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

        await pipelineContext.FileSystem.GetFile(stepSummaryVariable).WriteAsync($"{mermaid}\n\n{table}");
    }

    private async Task<string> GenerateMermaidSummary(PipelineSummary pipelineSummary)
    {
        var results = await pipelineSummary.GetModuleResultsAsync();

        var stepStringList = results
            .Where(x => x.ModuleDuration != TimeSpan.Zero)
            .OrderBy(x => x.ModuleEnd)
            .ThenBy(s => s.ModuleStart)
            .Select(x =>
            {
                var (startTime, endTime) = (x.ModuleStart, x.ModuleEnd);
                return $"{x.ModuleName} :{AddCritIfFailed(x)} {startTime:HH:mm:ss:fff}, {endTime:HH:mm:ss:fff}";
            }).ToList();

        var text = $"""
                    ```mermaid
                    ---
                    config:
                      theme: base
                      themeVariables:
                        primaryColor: "#007d15"
                        primaryTextColor: "#fff"
                        primaryBorderColor: "#02ad1e"
                        lineColor: "#F8B229"
                        secondaryColor: "#006100"
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
                    	axisFormat %M:%S

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
            .Select(stepContainer =>
                {
                    var (startTime, endTime, duration) = (stepContainer.ModuleStart, stepContainer.ModuleEnd, stepContainer.ModuleDuration);
                    var text = $"| {stepContainer.ModuleName} | {GetStatusString(stepContainer.ModuleStatus)} | {startTime:HH:mm:ss} | {endTime:HH:mm:ss} | {duration} |";
                    return text;
                }
            ).ToList();

        var (globalStartTime, globalEndTime, globalDuration) = (pipelineSummary.Start, pipelineSummary.End, pipelineSummary.TotalDuration);
        var pipelineStatusString = GetStatusString(pipelineSummary.Status);
        var overallSummaryString = $"| **Total** | **{pipelineStatusString}** | **{globalStartTime:HH:mm:ss}** | **{globalEndTime:HH:mm:ss}** | **{globalDuration}** |";
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
}