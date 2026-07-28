using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
#pragma warning disable CS0618 // This public compatibility attribute intentionally uses the legacy run-condition contract.
public class RunOnlyOnBranchAttribute : MandatoryRunConditionAttribute
{
    public string BranchName { get; }

    public RunOnlyOnBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}"));
    }
}
#pragma warning restore CS0618
