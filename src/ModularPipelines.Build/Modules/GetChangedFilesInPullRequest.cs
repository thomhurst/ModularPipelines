using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipIfDependabot]
[SkipOnMainBranch]
public class GetChangedFilesInPullRequest : Module<IReadOnlyList<PullRequestFile>>
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly GitHubClient _gitHubClient;

    public GetChangedFilesInPullRequest(IOptions<GitHubSettings> githubSettings, GitHubClient gitHubClient)
    {
        _githubSettings = githubSettings;
        _gitHubClient = gitHubClient;
    }

    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (!context.BuildSystemDetector.IsRunningOnGitHubActions)
        {
            return Task.FromResult(SkipDecision.Skip("Not running on GitHub actions"));
        }

        var isPullRequest = !string.IsNullOrEmpty(_githubSettings.Value.PullRequest?.Branch);
        return isPullRequest ? SkipDecision.DoNotSkip.AsTask() : SkipDecision.Skip("Not a pull request").AsTask();
    }

    protected override async Task<IReadOnlyList<PullRequestFile>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var pullRequestFiles = (await _gitHubClient
            .PullRequest
            .Files(_githubSettings.Value.Repository!.Id!.Value, _githubSettings.Value.PullRequest!.Number!.Value,
                new ApiOptions
                {
                    PageCount = 30,
                    PageSize = 100
                })
                ).ToList();

        if (pullRequestFiles.Count == 3000)
        {
            pullRequestFiles.AddRange(await _gitHubClient
                .PullRequest
                .Files(_githubSettings.Value.Repository!.Id!.Value, _githubSettings.Value.PullRequest!.Number!.Value,
                    new ApiOptions
                    {
                        PageCount = 30,
                        PageSize = 100,
                        StartPage = 2
                    }));
        }
        
        context.Logger.LogInformation("{Count} file changes", pullRequestFiles.Count);
        context.Logger.LogInformation("Changes files: {Files}", string.Join('|', pullRequestFiles.Select(x => x.FileName)));
        
        return pullRequestFiles;
    }
}