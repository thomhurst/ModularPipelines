using Microsoft.Extensions.Logging;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Helper class for branch condition checking logic used by branch-related attributes.
/// </summary>
internal static class BranchConditionHelper
{
    /// <summary>
    /// Checks if the current branch matches the expected branch name.
    /// </summary>
    internal static async Task<bool> CheckBranchMatches(
        IPipelineContext pipelineContext,
        string expectedBranchName,
        bool skip,
        CancellationToken cancellationToken = default)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false);
        var currentBranchName = repositoryInfo?.BranchName;
        if (pipelineContext.Logger.IsEnabled(LogLevel.Debug))
        {
            if (skip)
            {
                pipelineContext.Logger.LogDebug(
                    "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}",
                    GetDisplayBranchName(currentBranchName), expectedBranchName);
            }
            else
            {
                pipelineContext.Logger.LogDebug(
                    "Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}",
                    GetDisplayBranchName(currentBranchName), expectedBranchName);
            }
        }
        return currentBranchName == expectedBranchName;
    }

    /// <summary>
    /// Checks if the current branch starts with the expected prefix.
    /// </summary>
    internal static async Task<bool> CheckBranchStartsWith(
        IPipelineContext pipelineContext,
        string expectedPrefix,
        CancellationToken cancellationToken = default)
    {
        var repositoryInfo = await pipelineContext.Tools.Git.Information.GetInfoAsync(cancellationToken).ConfigureAwait(false);
        var currentBranchName = repositoryInfo?.BranchName;
        if (pipelineContext.Logger.IsEnabled(LogLevel.Debug))
        {
            pipelineContext.Logger.LogDebug(
                "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}",
                GetDisplayBranchName(currentBranchName), expectedPrefix);
        }
        return currentBranchName?.StartsWith(expectedPrefix, StringComparison.Ordinal) ?? false;
    }

    private static string GetDisplayBranchName(string? branchName)
    {
        return string.IsNullOrWhiteSpace(branchName) ? "(detached)" : branchName;
    }
}
