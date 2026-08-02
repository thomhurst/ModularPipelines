using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchStartsWithAttribute : RunIfAnyAttribute, IGroupedConditionAttribute
{
    public Type ConditionGroupType => typeof(BranchConditionHelper);

    public override string ConditionNames => $"{nameof(RunIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}");
    }
}
