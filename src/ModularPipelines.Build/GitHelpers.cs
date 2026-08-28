using Microsoft.Extensions.Options;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
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

        await context.Git().Commands.Repository.ConfigAsync(new GitConfigOptions
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

        await context.Git().Commands.Repository.ConfigAsync(new GitConfigOptions
        {
            Local = true,
            Arguments =
            [
                "user.email", settings.GitUserEmail,
            ],
        }, cancellationToken: cancellationToken);
    }

    internal static async Task<GitDiffDetails?> GetUncommittedChanges(
        IModuleContext context,
        IEnumerable<string> paths,
        CancellationToken cancellationToken)
    {
        var pathArguments = paths.ToArray();
        var changedFilesResult = await context.Git().Commands.WorkingTree.DiffAsync(
            new GitDiffOptions
            {
                NameOnly = true,
                Arguments = ["-z", "--", .. pathArguments],
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);
        var changedFiles = changedFilesResult.StandardOutput
            .Split('\0', StringSplitOptions.RemoveEmptyEntries);
        if (changedFiles.Length == 0)
        {
            return null;
        }

        var diffStatResult = await context.Git().Commands.WorkingTree.DiffAsync(
            new GitDiffOptions
            {
                Stat = true,
                Arguments = ["--", .. pathArguments],
            },
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new GitDiffDetails(changedFiles, diffStatResult.StandardOutput.TrimEnd());
    }

    private static GitHubSettings GetGitHubSettings(IModuleContext context)
    {
        var options = context.Services.GetRequiredService<IOptions<GitHubSettings>>();
        return options?.Value ?? new GitHubSettings();
    }
}

internal sealed record GitDiffDetails(IReadOnlyList<string> ChangedFiles, string DiffStat);
