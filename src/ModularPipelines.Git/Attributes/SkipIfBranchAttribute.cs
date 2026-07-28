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

    public override async Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return !await BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}").ConfigureAwait(false);
    }
}
#pragma warning restore CS0618
