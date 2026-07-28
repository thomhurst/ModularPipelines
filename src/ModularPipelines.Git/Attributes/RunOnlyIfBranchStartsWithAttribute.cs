using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunOnlyIfBranchStartsWithAttribute : Attribute, IGroupedConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Any;

    public Type ConditionGroupType => typeof(BranchConditionHelper);

    public string ConditionNames => $"{nameof(RunOnlyIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; }

    public RunOnlyIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return Task.FromResult(BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}"));
    }
}
