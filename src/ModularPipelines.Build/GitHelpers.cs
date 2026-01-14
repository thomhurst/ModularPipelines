using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub.Extensions;
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
            Arguments = new List<string>
            {
                "user.name", settings.GitUserName,
            },
        }, token: cancellationToken);
    }

    public static async Task SetEmail(IModuleContext context, CancellationToken cancellationToken)
    {
        var settings = GetGitHubSettings(context);

        await context.Git().Commands.Config(new GitConfigOptions
        {
            Local = true,
            Arguments = new List<string>
            {
                "user.email", settings.GitUserEmail,
            },
        }, token: cancellationToken);
    }

    public static async Task CheckoutBranch(IModuleContext context, string branchName, CancellationToken cancellationToken)
    {
        var settings = GetGitHubSettings(context);
        var token = settings.StandardToken;

        await context.Git().Commands.Remote(new GitRemoteOptions
        {
            Arguments =
            [
                "set-url", "origin",
                $"https://x-access-token:{token}@github.com/{settings.RepositoryOwner}/{settings.RepositoryName}"
            ],
        }, null, cancellationToken);

        await context.Git().Commands.Fetch(new GitFetchOptions(), token: cancellationToken);

        await context.Git().Commands
            .Checkout(new GitCheckoutOptions(branchName), token: cancellationToken);
    }

    public static async Task CommitAndPush(IModuleContext context, string? branchToPushTo, string message, string token,
        CancellationToken cancellationToken)
    {
        var settings = GetGitHubSettings(context);

        await context.Git().Commands.Pull(token: cancellationToken);

        await context.Git().Commands.Add(new GitAddOptions
        {
            All = true,
        }, token: cancellationToken);

        await context.Git().Commands.Commit(new GitCommitOptions
        {
            Message = message,
        }, token: cancellationToken);

        var author = context.GitHub().EnvironmentVariables.Actor ?? settings.RepositoryOwner;

        var arguments = new List<string> { $"https://x-access-token:{token}@github.com/{author}/{settings.RepositoryName}.git" };

        if (!string.IsNullOrEmpty(branchToPushTo))
        {
            arguments.Add(branchToPushTo);
        }

        await context.Git().Commands.Push(new GitPushOptions
        {
            Arguments = arguments,
        }, token: cancellationToken);
    }

    public static async Task<bool> HasUncommittedChanges(IModuleContext context)
    {
        var result = await context.Git().Commands.Diff(
            new GitDiffOptions
            {
                Quiet = true,
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