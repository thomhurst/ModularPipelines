using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchStartsWithAttribute : Attribute, IGroupedConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Any;

    public Type ConditionGroupType => typeof(BranchConditionHelper);

    public string ConditionNames => $"{nameof(RunIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}");
    }
}
