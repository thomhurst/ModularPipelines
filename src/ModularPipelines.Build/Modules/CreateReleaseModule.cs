using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.GitHub;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[SkipIfNoGitHubToken]
[RunOnlyOnBranch("main")]
[RunIf<ModularPipelines.OnLinux>]
[DependsOn<NugetVersionGeneratorModule>]
[DependsOn<UploadPackagesToNugetModule>]
public class CreateReleaseModule(IOptions<GitHubSettings> githubSettings,
    IOptions<PublishSettings> publishSettings) : Module<Release>
{
    private readonly IOptions<GitHubSettings> _githubSettings = githubSettings;
    private readonly IOptions<PublishSettings> _publishSettings = publishSettings;

#pragma warning disable MP0020 // Either missing publishing permission or a missing token must skip release creation.
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithSkipWhen(_ => !_publishSettings.Value.ShouldPublish, "The 'ShouldPublish' flag is false")
        .WithSkipWhen(_ => string.IsNullOrEmpty(_githubSettings.Value.AdminToken), "The GitHub admin token is unavailable")
        .WithIgnoreFailuresWhen((_, ex) => ex is ApiValidationException);
#pragma warning restore MP0020

    protected override async Task<Release> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInfoResult = await context.GetModule<NugetVersionGeneratorModule>();

        var repositoryIdString = context.Tools.GitHub.EnvironmentVariables.RepositoryId;
        if (!long.TryParse(repositoryIdString, out var repositoryId))
        {
            throw new InvalidOperationException($"Failed to parse RepositoryId '{repositoryIdString}' as a valid long integer.");
        }

        var version = versionInfoResult.Value;
        var release = new NewRelease($"v{version}")
        {
            Name = version,
            GenerateReleaseNotes = true,
            Prerelease = version.Split('+')[0].Contains('-'),
        };

        if (version == "4.0.0")
        {
            using var notesStream = typeof(CreateReleaseModule).Assembly.GetManifestResourceStream("ModularPipelines.Build.RELEASE_NOTES_V4.md")
                ?? throw new InvalidOperationException("The curated v4 release notes are missing from the build assembly.");
            using var reader = new StreamReader(notesStream);
            release.Body = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
            release.GenerateReleaseNotes = false;
        }

        return await context.Tools.GitHub.Client.Repository.Release.Create(repositoryId, release).ConfigureAwait(false);
    }
}
