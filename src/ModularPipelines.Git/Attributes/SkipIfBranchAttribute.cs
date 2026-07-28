using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfBranchAttribute : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => $"{nameof(SkipIfBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public SkipIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return Task.FromResult(BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}"));
    }
}
