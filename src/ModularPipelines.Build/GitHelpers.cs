using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Build;

public static class GitHelpers
{
    public static async Task SetUserCommitInformation(IModuleContext context, CancellationToken cancellationToken)
    {
        await SetName(context, cancellationToken);
        await SetEmail(context, cancellationToken);
    }

    public static async Task SetName(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = GetGitHubSettings(context);

        await context.Tools.Git.Commands.Repository.ConfigAsync(new GitConfigOptions
        {
            Local = true,
            Arguments =
            [
                "user.name", settings.GitUserName,
            ],
        }, cancellationToken: cancellationToken);
    }

    public static async Task SetEmail(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = GetGitHubSettings(context);

        await context.Tools.Git.Commands.Repository.ConfigAsync(new GitConfigOptions
        {
            Local = true,
            Arguments =
            [
                "user.email", settings.GitUserEmail,
            ],
        }, cancellationToken: cancellationToken);
    }

    private static GitHubSettings GetGitHubSettings(IModuleContext context)
    {
        var options = context.Services.GetRequiredService<IOptions<GitHubSettings>>();
        return options?.Value ?? new GitHubSettings();
    }
}
