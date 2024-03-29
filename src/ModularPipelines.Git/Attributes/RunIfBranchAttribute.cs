using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class RunIfBranchAttribute : RunConditionAttribute
{
    public string BranchName { get; }

    public RunIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName == BranchName);
    }
}