using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

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
        return Task.FromResult(BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}"));
    }
}
