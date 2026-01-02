using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class SkipIfBranchAttribute : MandatoryRunConditionAttribute
{
    public string BranchName { get; }

    public SkipIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(!BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}"));
    }
}
