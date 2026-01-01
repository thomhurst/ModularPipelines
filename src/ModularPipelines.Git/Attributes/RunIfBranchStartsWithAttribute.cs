using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class RunIfBranchStartsWithAttribute : RunConditionAttribute
{
    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var currentBranchName = pipelineContext.Git().Information.BranchName;

        pipelineContext.Logger.LogDebug("Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}", currentBranchName, BranchNamePrefix);

        return Task.FromResult(currentBranchName?.StartsWith(BranchNamePrefix) ?? false);
    }
}