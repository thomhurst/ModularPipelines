using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[SkipIfDependabot]
[DependsOn<UploadPackagesToNugetModule>]
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

    protected override async Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        await Task.Yield();
        return exception is ApiValidationException;
    }

    /// <inheritdoc/>
    protected override async Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (!_publishSettings.Value.ShouldPublish)
        {
            return true;
        }

        var releaseNotesFile = context.Git()
                .RootDirectory
                .FindFile(x => x.Name == "ReleaseNotes.md");

        return
            string.IsNullOrEmpty(_githubSettings.Value.AdminToken)
            || releaseNotesFile?.Exists != true
            || string.IsNullOrWhiteSpace(await releaseNotesFile.ReadAsync());
    }

    /// <inheritdoc/>
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var releaseNotesFile = context.Git().RootDirectory.FindFile(x => x.Name == "ReleaseNotes.md")!;

        var releaseNotesContents = await releaseNotesFile.ReadAsync(cancellationToken);

        var versionInfoResult = await GetModule<NugetVersionGeneratorModule>();

        if (!releaseNotesContents.Trim().Equals("null", StringComparison.OrdinalIgnoreCase))
        {
            await _gitHubClient.Repository.Release.Create(_githubSettings.Value.Repository!.Id!.Value,
                new NewRelease(versionInfoResult.Value)
                {
                    Name = versionInfoResult.Value,
                    Body = releaseNotesContents,
                });
        }

        await ResetReleaseNotesFile(releaseNotesFile, context, cancellationToken);

        return await NothingAsync();
    }

    private async Task ResetReleaseNotesFile(File releaseNotesFile, IPipelineContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await releaseNotesFile.WriteAsync(string.Empty, cancellationToken);

            await GitHelpers.SetUserCommitInformation(context, cancellationToken);
            await GitHelpers.CommitAndPush(context, null, "Create Release", _githubSettings.Value.AdminToken!, cancellationToken);
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(e, "Error pushing");
        }
    }
}