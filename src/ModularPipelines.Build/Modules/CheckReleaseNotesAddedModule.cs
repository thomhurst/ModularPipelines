using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[RunOnLinux, SkipIfDependabot, SkipOnMainBranch]
public class CheckReleaseNotesAddedModule : Module
{
    private const string MissingReleaseNotesMessage = "No release notes for this change. Please add some notes to the ReleaseNotes.md file.";
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly GitHubClient _gitHubClient;

    public CheckReleaseNotesAddedModule(IOptions<GitHubSettings> githubSettings, GitHubClient gitHubClient)
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

    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var releaseNotesFile = context.Git().RootDirectory.GetFolder("src").GetFolder("ModularPipelines.Build").GetFile("ReleaseNotes.md");
        
        if (!releaseNotesFile.Exists
            || string.IsNullOrEmpty(await releaseNotesFile.ReadAsync())
            || !await ReleaseNotesChangedInPullRequest())
        {
            throw new Exception(MissingReleaseNotesMessage);
        }

        return await NothingAsync();
    }

    private async Task<bool> ReleaseNotesChangedInPullRequest()
    {
        var pullRequestFiles = await _gitHubClient.PullRequest.Files(_githubSettings.Value.Repository!.Id!.Value,
            _githubSettings.Value.PullRequest!.Number!.Value);

        return pullRequestFiles.Any(x => x.FileName == "src/ModularPipelines.Build/ReleaseNotes.md");
    }
}