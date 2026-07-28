using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

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
        string logMessageFormat)
    {
        var repositoryInfo = await pipelineContext.Git().Information.GetInfoAsync().ConfigureAwait(false);
        var currentBranchName = repositoryInfo?.BranchName;
        pipelineContext.Logger.LogDebug(logMessageFormat, GetDisplayBranchName(currentBranchName), expectedBranchName);
        return currentBranchName == expectedBranchName;
    }

    /// <summary>
    /// Checks if the current branch starts with the expected prefix.
    /// </summary>
    internal static async Task<bool> CheckBranchStartsWith(
        IPipelineContext pipelineContext,
        string expectedPrefix,
        string logMessageFormat)
    {
        var repositoryInfo = await pipelineContext.Git().Information.GetInfoAsync().ConfigureAwait(false);
        var currentBranchName = repositoryInfo?.BranchName;
        pipelineContext.Logger.LogDebug(logMessageFormat, GetDisplayBranchName(currentBranchName), expectedPrefix);
        return currentBranchName?.StartsWith(expectedPrefix) ?? false;
    }

    private static string GetDisplayBranchName(string? branchName)
    {
        return string.IsNullOrWhiteSpace(branchName) ? "(detached)" : branchName;
    }
}
