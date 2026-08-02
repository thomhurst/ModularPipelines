using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchAttribute : Attribute, IGroupedConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Any;

    public Type ConditionGroupType => typeof(BranchConditionHelper);

    public string ConditionNames => $"{nameof(RunIfBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public RunIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return EvaluateAsync(pipelineContext, default);
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}",
            cancellationToken);
    }
}
