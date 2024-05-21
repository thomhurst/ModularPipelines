using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
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
    private readonly IOptions<GitHubSettings> _gitHubSettings;
    private readonly IGitHubClient _gitHubClient;

    public WaitForOtherOperatingSystemBuilds(IOptions<GitHubSettings> gitHubSettings,
        IGitHubClient gitHubClient)
    {
        _gitHubSettings = gitHubSettings;
        _gitHubClient = gitHubClient;
    }

    /// <inheritdoc/>
    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        return context.Git().Information.BranchName != "main" && _gitHubSettings.Value?.PullRequest?.Sha is null
            ? SkipDecision.Skip("No github commit sha found").AsTask()
            : SkipDecision.DoNotSkip.AsTask();
    }

    /// <inheritdoc/>
    protected override async Task<List<WorkflowRun>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var commitSha = _gitHubSettings.Value.PullRequest?.Sha ?? context.Git().Information.LastCommitSha;

        var windowsRuns = await _gitHubClient.Actions.Workflows.Runs.ListByWorkflow(BuildConstants.Owner, BuildConstants.RepositoryName, "dotnet-windows.yml", new WorkflowRunsRequest
        {
            HeadSha = commitSha,
        });
        
        var macRuns = await _gitHubClient.Actions.Workflows.Runs.ListByWorkflow(BuildConstants.Owner, BuildConstants.RepositoryName, "dotnet-mac.yml", new WorkflowRunsRequest
        {
            HeadSha = commitSha,
        });

        var windowsRun = windowsRuns.WorkflowRuns.FirstOrDefault(x => x.HeadSha == commitSha);
        var macRun = macRuns.WorkflowRuns.FirstOrDefault(x => x.HeadSha == commitSha);

        var waitForWindows = await WaitFor(windowsRun, cancellationToken);
        var waitForMac = await WaitFor(macRun, cancellationToken);

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

    private async Task<WorkflowRun?> WaitFor(WorkflowRun? workflowRun, CancellationToken cancellationToken)
    {
        if (workflowRun == null)
        {
            return null;
        }

        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var run = await _gitHubClient.Actions.Workflows.Runs.Get(BuildConstants.Owner, BuildConstants.RepositoryName, workflowRun.Id);

            if (run?.Conclusion.HasValue is true)
            {
                return run;
            }

            await Task.Delay(TimeSpan.FromSeconds(30), cancellationToken);
        }
    }
}