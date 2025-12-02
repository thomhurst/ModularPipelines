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
using ModularPipelines.Modules.Behaviors;
using Octokit;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<UploadPackagesToNugetModule>]
[DependsOn<DependabotCommitsModule>]
public class CreateReleaseModule : Module<Release>, ISkippable, IIgnoreFailures
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly IOptions<PublishSettings> _publishSettings;

    public CreateReleaseModule(IOptions<GitHubSettings> githubSettings,
        IOptions<PublishSettings> publishSettings)
    {
        _githubSettings = githubSettings;
        _publishSettings = publishSettings;
    }

    public async Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception)
    {
        await Task.Yield();
        return exception is ApiValidationException;
    }

    public async Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        await Task.CompletedTask;

        if (!_publishSettings.Value.ShouldPublish)
        {
            return "The 'ShouldPublish' flag is false";
        }

        return string.IsNullOrEmpty(_githubSettings.Value.AdminToken);
    }

    public override async Task<Release?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInfoResult = context.GetModule<NugetVersionGeneratorModule, string>();

        return await context.GitHub().Client.Repository.Release.Create(long.Parse(context.GitHub().EnvironmentVariables.RepositoryId!),
            new NewRelease($"v{versionInfoResult.Value}")
            {
                Name = versionInfoResult.Value,
                GenerateReleaseNotes = true,
            });
    }
}