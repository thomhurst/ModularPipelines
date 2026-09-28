using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfBranchAttribute : RunConditionAttribute
{
    public override string ConditionNames => $"{nameof(SkipIfBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public SkipIfBranchAttribute(string branchName)
        : base(ConditionIntent.Skip)
    {
        BranchName = branchName;
    }

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}",
            cancellationToken);
    }
}
