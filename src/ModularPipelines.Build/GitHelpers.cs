using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

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

        await context.Git().Commands.Config(new GitConfigOptions
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

        await context.Git().Commands.Config(new GitConfigOptions
        {
            Local = true,
            Arguments =
            [
                "user.email", settings.GitUserEmail,
            ],
        }, cancellationToken: cancellationToken);
    }

    public static async Task<bool> HasUncommittedChanges(IModuleContext context, IEnumerable<string> paths)
    {
        var result = await context.Git().Commands.Diff(
            new GitDiffOptions
            {
                Quiet = true,
                Arguments = ["--", .. paths],
            },
            new CommandExecutionOptions
            {
                ThrowOnNonZeroExitCode = false,
            });

        return result.ExitCode != 0;
    }

    private static GitHubSettings GetGitHubSettings(IModuleContext context)
    {
        var options = context.Services.Get<IOptions<GitHubSettings>>();
        return options?.Value ?? new GitHubSettings();
    }
}
