using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[DependsOn<RunUnitTestsModule>]
[DependsOn<PackProjectsModule>]
public class WaitForOtherOperatingSystemBuilds : Module<List<WorkflowRun>>
{
    /// <inheritdoc/>
    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return string.IsNullOrEmpty(context.GitHub().EnvironmentVariables.Sha)
            ? SkipDecision.Skip("No github commit sha found").AsTask()
            : SkipDecision.DoNotSkip.AsTask();
    }

    /// <inheritdoc/>
    protected override async Task<List<WorkflowRun>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var commitSha = context.GitHub().EnvironmentVariables.Sha;

        var windowsRuns = await context.GitHub().Client.Actions.Workflows.Runs.ListByWorkflow(BuildConstants.Owner, BuildConstants.RepositoryName, "dotnet-windows.yml", new WorkflowRunsRequest
        {
            HeadSha = commitSha,
        });
        
        var macRuns = await context.GitHub().Client.Actions.Workflows.Runs.ListByWorkflow(BuildConstants.Owner, BuildConstants.RepositoryName, "dotnet-mac.yml", new WorkflowRunsRequest
        {
            HeadSha = commitSha,
        });

        var windowsRun = windowsRuns.WorkflowRuns.FirstOrDefault(x => x.HeadSha == commitSha);
        var macRun = macRuns.WorkflowRuns.FirstOrDefault(x => x.HeadSha == commitSha);

        var waitForWindows = await WaitFor(context.GitHub().Client, windowsRun, cancellationToken);
        var waitForMac = await WaitFor(context.GitHub().Client, macRun, cancellationToken);

        var list = new List<WorkflowRun>();

        if (waitForWindows != null)
        {
            list.Add(waitForWindows);
        }

        if (waitForMac != null)
        {
            list.Add(waitForMac);
        }

        return list;
    }

    private async Task<WorkflowRun?> WaitFor(IGitHubClient client, WorkflowRun? workflowRun,
        CancellationToken cancellationToken, [CallerArgumentExpression("workflowRun")] string expression = "")
    {
        if (workflowRun == null)
        {
            Context.Logger.LogInformation("No workflow found for {Expression}", expression);
            return null;
        }

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var run = await client.Actions.Workflows.Runs.Get(BuildConstants.Owner, BuildConstants.RepositoryName, workflowRun.Id);

            if (run?.Conclusion.HasValue is true)
            {
                return run;
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }
}