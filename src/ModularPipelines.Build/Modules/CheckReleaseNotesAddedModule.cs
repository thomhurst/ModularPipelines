using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

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

    protected override Task<bool> ShouldSkip(IModuleContext context)
    {
        if (!context.BuildSystemDetector.IsRunningOnGitHubActions)
        {
            return Task.FromResult(true);
        }

        if (_githubSettings.Value.Actor == "dependabot[bot]")
        {
            return Task.FromResult(true);
        }
        
        return Task.FromResult(string.IsNullOrEmpty(_githubSettings.Value.PullRequest?.Branch));
    }

    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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