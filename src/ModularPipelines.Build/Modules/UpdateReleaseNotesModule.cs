﻿using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
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

    public UpdateReleaseNotesModule(IOptions<GitHubSettings> githubSettings, 
        GitHubClient gitHubClient,
        IOptions<PublishSettings> publishSettings)
    {
        _githubSettings = githubSettings;
        _gitHubClient = gitHubClient;
    }

    protected override Task<bool> ShouldSkip(IModuleContext context)
    {
        return Task.FromResult(string.IsNullOrEmpty(_githubSettings.Value.AdminToken));
    }

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var releaseNotesFile = context.Git().RootDirectory.GetFolder("src").GetFolder("ModularPipelines.Build").GetFile("ReleaseNotes.md");

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
        await GitHelpers.CommitAndPush(context, null, "Create Release", _githubSettings.Value.AdminToken!, cancellationToken);
    }
}