using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunIf<ModularPipelines.OnLinux>]
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

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(_ => !_publishSettings.Value.ShouldPublish, "The 'ShouldPublish' flag is false")
        .WithSkipWhen(_ => string.IsNullOrEmpty(_githubSettings.Value.AdminToken), "The GitHub admin token is unavailable")
        .WithIgnoreFailuresWhen((_, ex) => ex is ApiValidationException);

    protected override async Task<Release> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInfoResult = await context.GetModule<NugetVersionGeneratorModule>();

        var repositoryIdString = context.Tools.GitHub.EnvironmentVariables.RepositoryId;
        if (!long.TryParse(repositoryIdString, out var repositoryId))
        {
            throw new InvalidOperationException($"Failed to parse RepositoryId '{repositoryIdString}' as a valid long integer.");
        }

        return await context.Tools.GitHub.Client.Repository.Release.Create(repositoryId,
            new NewRelease($"v{versionInfoResult.Value}")
            {
                Name = versionInfoResult.Value,
                GenerateReleaseNotes = true,
            });
    }
}
