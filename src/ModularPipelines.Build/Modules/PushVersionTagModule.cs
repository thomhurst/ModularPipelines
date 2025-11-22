using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Attributes;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules;

[RunOnlyOnBranch("main")]
[RunOnLinuxOnly]
[DependsOn<NugetVersionGeneratorModule>]
public class PushVersionTagModule : Module<CommandResult>, IModuleErrorHandling
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public PushVersionTagModule(IOptions<GitHubSettings> gitHubSettings)
    {
        _gitHubSettings = gitHubSettings;
    }

    public async Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)
    {
        var versionInformation = await context.GetModuleAsync<NugetVersionGeneratorModule>();

        return exception.Message.Contains($"tag 'v{versionInformation.Value!}' already exists");
    }

    public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var versionInformation = await context.GetModuleAsync<NugetVersionGeneratorModule>();

        await context.Git().Commands.Tag(new GitTagOptions
        {
            Arguments = [$"v{versionInformation.Value!}"],
        }, cancellationToken);

        var token = _gitHubSettings.Value.StandardToken;
        var author = context.GitHub().EnvironmentVariables.Actor ?? "thomhurst";
        var authenticatedRemoteUrl = $"https://x-access-token:{token}@github.com/{author}/ModularPipelines.git";

        return await context.Git().Commands.Push(new GitPushOptions
        {
            Arguments = [authenticatedRemoteUrl, "--tags"],
        }, cancellationToken);
    }
}
