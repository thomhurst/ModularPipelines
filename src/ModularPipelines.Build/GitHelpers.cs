using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub.Extensions;

namespace ModularPipelines.Build;

public static class GitHelpers
{
    public static async Task SetUserCommitInformation(IPipelineContext context, CancellationToken cancellationToken)
    {
        await SetName(context, cancellationToken);
        await SetEmail(context, cancellationToken);
    }

    public static async Task SetName(IPipelineContext context, CancellationToken cancellationToken)
    {
        await context.Git().Commands.Config(new GitConfigOptions
        {
            Local = true,
            Arguments = new List<string>
            {
                "user.name", "Tom Longhurst",
            },
        }, cancellationToken);
    }

    public static async Task SetEmail(IPipelineContext context, CancellationToken cancellationToken)
    {
        await context.Git().Commands.Config(new GitConfigOptions
        {
            Local = true,
            Arguments = new List<string>
            {
                "user.email", "thomhurst@users.noreply.github.com",
            },
        }, cancellationToken);
    }

    public static async Task CheckoutBranch(IPipelineContext context, string branchName, CancellationToken cancellationToken)
    {
        var options = context.Get<IOptions<GitHubSettings>>();

        var token = options!.Value.StandardToken;

        await context.Git().Commands.Remote(new GitRemoteOptions
        {
            Arguments =
            [
                "set-url", "origin",
                $"https://x-access-token:{token}@github.com/thomhurst/ModularPipelines"
            ],
        }, cancellationToken);

        await context.Git().Commands.Fetch(new GitFetchOptions(), token: cancellationToken);

        await context.Git().Commands
            .Checkout(new GitCheckoutOptions(branchName), cancellationToken);
    }

    public static async Task CommitAndPush(IPipelineContext context, string? branchToPushTo, string message, string token,
        CancellationToken cancellationToken)
    {
        await context.Git().Commands.Pull(token: cancellationToken);

        await context.Git().Commands.Add(new GitAddOptions
        {
            All = true,
        }, token: cancellationToken);

        await context.Git().Commands.Commit(new GitCommitOptions
        {
            Message = message,
        }, token: cancellationToken);
        
        var author = context.GitHub().EnvironmentVariables.Actor ?? "thomhurst";

        var arguments = new List<string> { $"https://x-access-token:{token}@github.com/{author}/ModularPipelines.git" };

        if (!string.IsNullOrEmpty(branchToPushTo))
        {
            arguments.Add(branchToPushTo);
        }

        await context.Git().Commands.Push(new GitPushOptions
        {
            Arguments = arguments,
        }, token: cancellationToken);
    }

    public static async Task<bool> HasUncommittedChanges(IPipelineContext context)
    {
        var result = await context.Git().Commands.Diff(new GitDiffOptions
        {
            Quiet = true,
            ThrowOnNonZeroExitCode = false,
        });

        return result.ExitCode != 0;
    }
}