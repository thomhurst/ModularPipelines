using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchAttribute : RunConditionAttribute
{
    public override Type? GroupKey => typeof(BranchConditionHelper);

    public override string ConditionNames => $"{nameof(RunIfBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public RunIfBranchAttribute(string branchName)
        : base(ConditionIntent.Run)
    {
        BranchName = branchName;
    }

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Can run on: {ExpectedBranch}",
            cancellationToken);
    }
}
