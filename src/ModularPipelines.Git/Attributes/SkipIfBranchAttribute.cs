using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
#pragma warning disable CS0618 // This public compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipIfBranchAttribute : MandatoryRunConditionAttribute
{
    public string BranchName { get; }

    public SkipIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(!BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}"));
    }
}
#pragma warning restore CS0618
