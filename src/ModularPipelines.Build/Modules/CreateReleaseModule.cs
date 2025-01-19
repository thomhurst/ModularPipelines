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

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<UploadPackagesToNugetModule>]
[DependsOn<DependabotCommitsModule>]
public class CreateReleaseModule : Module<Release>
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public CreateReleaseModule(IOptions<GitHubSettings> githubSettings,
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
        await Task.CompletedTask;
        
        if (!_publishSettings.Value.ShouldPublish)
        {
            return "The 'ShouldPublish' flag is false";
        }

        return string.IsNullOrEmpty(_githubSettings.Value.AdminToken);
    }

    /// <inheritdoc/>
    protected override async Task<Release?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var versionInfoResult = await GetModule<NugetVersionGeneratorModule>();
        
        return await context.GitHub().Client.Repository.Release.Create(long.Parse(context.GitHub().EnvironmentVariables.RepositoryId!),
            new NewRelease($"v{versionInfoResult.Value}")
            {
                Name = versionInfoResult.Value,
                GenerateReleaseNotes = true,
            });
    }
}