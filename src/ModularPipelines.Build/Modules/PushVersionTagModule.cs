using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Build.Modules;

[ModuleCategory("VersionTag")]
[SkipIfNoStandardGitHubToken]
[RunOnlyOnBranch("main")]
[RunIf<ModularPipelines.OnLinux>]
[DependsOn<NugetVersionGeneratorModule>]
public class PushVersionTagModule : Module<CommandResult>
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public PushVersionTagModule(IOptions<GitHubSettings> gitHubSettings)
    {
        _gitHubSettings = gitHubSettings;
    }

    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithIgnoreFailuresWhen((ctx, ex) =>
        {
            // Use GetAwaiter().GetResult() since dependency has completed by the time this callback runs
            var versionInformation = ((IModuleContext)ctx).GetModule<NugetVersionGeneratorModule>().GetAwaiter().GetResult();
            return ex.Message.Contains($"tag 'v{versionInformation.Value}' already exists");
        });

    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var versionInformation = await context.GetModule<NugetVersionGeneratorModule>();

        // Configure git user information
        await GitHelpers.SetUserCommitInformation(context, cancellationToken);

        // Configure remote URL with authentication token
        var token = _gitHubSettings.Value.StandardToken!;

        await context.Tools.Git.Commands.Remotes.RemoteAsync(new GitRemoteOptions
        {
            Arguments =
            [
                "set-url", "origin",
                $"https://x-access-token:{token}@github.com/{_gitHubSettings.Value.RepositoryOwner}/{_gitHubSettings.Value.RepositoryName}"
            ],
        }, null, cancellationToken);

        await context.Tools.Git.Commands.Branches.TagAsync(new GitTagOptions
        {
            TagName = $"v{versionInformation.Value}",
        }, cancellationToken: cancellationToken);

        return await context.Tools.Git.Commands.Remotes.PushAsync(
            new GitPushOptions
            {
                Tags = true,
            },
            new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = false,
            },
            cancellationToken: cancellationToken);
    }
}
