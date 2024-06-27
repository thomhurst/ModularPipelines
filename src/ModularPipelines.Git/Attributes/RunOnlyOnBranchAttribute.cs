using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnlyOnBranchAttribute : MandatoryRunConditionAttribute
{
    public string BranchName { get; }

    public RunOnlyOnBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var currentBranchName = pipelineContext.Git().Information.BranchName;
        
        pipelineContext.Logger.LogDebug("Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}", currentBranchName, BranchName);
        
        return Task.FromResult(currentBranchName == BranchName);
    }
}