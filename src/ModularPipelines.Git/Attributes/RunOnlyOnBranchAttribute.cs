using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunOnlyOnBranchAttribute : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.All;

    public string ConditionNames => $"{nameof(RunOnlyOnBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public RunOnlyOnBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}");
    }
}
