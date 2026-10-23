using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

[RequiresCapability("ci-master")]
[ModuleCategory("VersionTag")]
[RunIf<ExplicitVersionTag>]
[SkipIfNoStandardGitHubToken]
[RunOnlyOnBranch("main")]
[RunIf<ModularPipelines.OnLinux>]
[DependsOn<NugetVersionGeneratorModule>]
public class PushVersionTagModule(IOptions<GitHubSettings> gitHubSettings) : Module<CommandResult>
{
    private readonly IOptions<GitHubSettings> _gitHubSettings = gitHubSettings;

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInformation = await context.GetModule<NugetVersionGeneratorModule>();

        // Configure git user information
        await GitHelpers.SetUserCommitInformation(context, cancellationToken).ConfigureAwait(false);

        // Configure remote URL with authentication token
        var token = _gitHubSettings.Value.StandardToken!;

        await context.Tools.Git.Commands.Remotes.RemoteAsync(new GitRemoteOptions
        {
            Arguments =
            [
                "set-url", "origin",
                $"https://x-access-token:{token}@github.com/{_gitHubSettings.Value.RepositoryOwner}/{_gitHubSettings.Value.RepositoryName}"
            ],
        }, null, cancellationToken).ConfigureAwait(false);

        return await context.Tools.Git.Commands.Remotes.PushAsync(
            new GitPushOptions
            {
                // Push one lightweight tag directly. An identical retry is safe, and Git rejects
                // a version that already points at a different commit without moving it.
                Arguments = ["origin", $"HEAD:refs/tags/v{versionInformation.Value}"],
            },
            new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = true,
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
