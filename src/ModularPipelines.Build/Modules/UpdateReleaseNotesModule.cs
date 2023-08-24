using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<NugetVersionGeneratorModule>]
public class UpdateReleaseNotesModule : Module
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly GitHubClient _gitHubClient;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UpdateReleaseNotesModule(IOptions<GitHubSettings> githubSettings, 
        GitHubClient gitHubClient,
        IOptions<PublishSettings> publishSettings)
    {
        _githubSettings = githubSettings;
        _gitHubClient = gitHubClient;
        _publishSettings = publishSettings;
    }

    protected override async Task<bool> ShouldSkip(IModuleContext context)
    {
        if (!context.BuildSystemDetector.IsRunningOnGitHubActions)
        {
            return true;
        }

        if (!_publishSettings.Value.ShouldPublish)
        {
            return true;
        }

        var gitVersioningInfo = await context.Git().Versioning.GetGitVersioningInformation();
        
        return gitVersioningInfo.BranchName != "main";
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var releaseNotesFile = context.Environment.GitRootDirectory!.GetFolder("ModularPipelines.Build").GetFile("ReleaseNotes.md");

        var releaseNotesContents = await releaseNotesFile.ReadAsync();

        if (string.IsNullOrEmpty(releaseNotesContents))
        {
            return await NothingAsync();
        }
        
        var versionInfoResult = await GetModule<NugetVersionGeneratorModule>();

        await _gitHubClient.Repository.Release.Create(_githubSettings.Value.Repository!.Id!.Value,
            new NewRelease(versionInfoResult.Value)
            {
                Name = versionInfoResult.Value,
                Body = releaseNotesContents
            });
        
        await ResetReleaseNotesFile(releaseNotesFile, context, cancellationToken);

        return await NothingAsync();
    }

    private async Task ResetReleaseNotesFile(File releaseNotesFile, IModuleContext context,
        CancellationToken cancellationToken)
    {
        await releaseNotesFile.WriteAsync(string.Empty);
        
        await GitHelpers.SetUserCommitInformation(context, cancellationToken);
        await GitHelpers.SetTarget(context, "main", _githubSettings.Value.TokenWithoutTriggerBuild!, cancellationToken);
        await GitHelpers.CommitAndPush(context, "main", "Create Release", cancellationToken);
    }
}