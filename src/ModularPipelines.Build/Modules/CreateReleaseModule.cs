using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
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

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(() =>
        {
            if (!_publishSettings.Value.ShouldPublish)
            {
                return SkipDecision.Skip("The 'ShouldPublish' flag is false");
            }

            return string.IsNullOrEmpty(_githubSettings.Value.AdminToken);
        })
        .WithIgnoreFailuresWhen((_, ex) => ex is ApiValidationException)
        .Build();

    protected override async Task<Release?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInfoResult = await context.GetModule<NugetVersionGeneratorModule>();

        var repositoryIdString = context.GitHub().EnvironmentVariables.RepositoryId;
        if (!long.TryParse(repositoryIdString, out var repositoryId))
        {
            throw new InvalidOperationException($"Failed to parse RepositoryId '{repositoryIdString}' as a valid long integer.");
        }

        return await context.GitHub().Client.Repository.Release.Create(repositoryId,
            new NewRelease($"v{versionInfoResult.ValueOrDefault}")
            {
                Name = versionInfoResult.ValueOrDefault,
                GenerateReleaseNotes = true,
            });
    }
}
