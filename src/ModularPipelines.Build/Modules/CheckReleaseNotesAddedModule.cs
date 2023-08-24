using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;
using Octokit.Internal;

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
        
        return Task.FromResult(string.IsNullOrEmpty(_githubSettings.Value.PullRequest?.Branch));
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var releaseNotesFile = context.Environment.GitRootDirectory!.GetFile("ReleaseNotes.md");
        
        if (!releaseNotesFile.Exists
            || string.IsNullOrEmpty(await releaseNotesFile.ReadAsync())
            || !await ReleaseNotesChangedInPullRequest(context))
        {
            throw new Exception(MissingReleaseNotesMessage);
        }

        return await NothingAsync();
    }

    private async Task<bool> ReleaseNotesChangedInPullRequest(IModuleContext moduleContext)
    {
        var pullRequestFiles = await _gitHubClient.PullRequest.Files(_githubSettings.Value.Repository!.Id!.Value,
            _githubSettings.Value.PullRequest!.Number!.Value);
        
        foreach (var pullRequestFile in pullRequestFiles)
        {
            Console.WriteLine(pullRequestFile.FileName);
        }

        return pullRequestFiles.Any(x => x.FileName.EndsWith("ReleaseNotes.md"));
    }
}