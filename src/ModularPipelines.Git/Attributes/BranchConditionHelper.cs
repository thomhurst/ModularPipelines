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
    internal static bool CheckBranchMatches(
        IPipelineHookContext pipelineContext,
        string expectedBranchName,
        string logMessageFormat)
    {
        var currentBranchName = pipelineContext.Git().Information.BranchName;
        pipelineContext.Logger.LogDebug(logMessageFormat, currentBranchName, expectedBranchName);
        return currentBranchName == expectedBranchName;
    }

    /// <summary>
    /// Checks if the current branch starts with the expected prefix.
    /// </summary>
    internal static bool CheckBranchStartsWith(
        IPipelineHookContext pipelineContext,
        string expectedPrefix,
        string logMessageFormat)
    {
        var currentBranchName = pipelineContext.Git().Information.BranchName;
        pipelineContext.Logger.LogDebug(logMessageFormat, currentBranchName, expectedPrefix);
        return currentBranchName?.StartsWith(expectedPrefix) ?? false;
    }
}
