using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using Octokit;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<UploadPackagesToNugetModule>]
[DependsOn<DependabotCommitsModule>]
public class UpdateReleaseNotesModule : Module
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public UpdateReleaseNotesModule(IOptions<GitHubSettings> githubSettings,
        IOptions<PublishSettings> publishSettings)
    {
        _githubSettings = githubSettings;
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
            return "The 'ShouldPublish' flag is false";
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

        var releaseNotesContents = await GetReleaseNotesContents(cancellationToken, releaseNotesFile);

        var versionInfoResult = await GetModule<NugetVersionGeneratorModule>();
        
        if (!string.IsNullOrWhiteSpace(releaseNotesContents.Trim()))
        {
            await context.GitHub().Client.Repository.Release.Create(long.Parse(context.GitHub().EnvironmentVariables.RepositoryId!),
                new NewRelease(versionInfoResult.Value)
                {
                    Name = versionInfoResult.Value,
                    Body = releaseNotesContents,
                });
        }

        await ResetReleaseNotesFile(releaseNotesFile, context, cancellationToken);

        return await NothingAsync();
    }

    private async Task<string> GetReleaseNotesContents(CancellationToken cancellationToken, File releaseNotesFile)
    {
        var customNotes = await releaseNotesFile.ReadAsync(cancellationToken);

        if (string.Equals("null", customNotes.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            customNotes = string.Empty;
        }

        customNotes += await GetDependabotCommits();
        
        return customNotes.Trim();
    }

    private async Task<string> GetDependabotCommits()
    {
        var commits = await GetModule<DependabotCommitsModule>();

        if (commits.Value?.Any() != true)
        {
            return string.Empty;
        }

        var splitCommits = string.Join(Environment.NewLine,
            commits.Value.Select(x => $"*   {x}")
        );

        return $"""
                
                ## Dependencies

                {splitCommits}
                """;
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