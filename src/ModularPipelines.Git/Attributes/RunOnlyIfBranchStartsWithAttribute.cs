using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class RunOnlyIfBranchStartsWithAttribute : MandatoryRunConditionAttribute
{
    public string BranchNamePrefix { get; }

    public RunOnlyIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}"));
    }
}
