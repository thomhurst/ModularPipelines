using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Requires a module to run on the specified branch.
/// </summary>
/// <remarks>
/// Each instance is an independent AND requirement, including alongside grouped RunIfBranch and RunIfBranchStartsWith alternatives.
/// </remarks>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunOnlyOnBranchAttribute(string branchName) : RunConditionAttribute(ConditionIntent.Run)
{
    public override string ConditionNames => $"{nameof(RunOnlyOnBranchAttribute)}({BranchName})";

    public string BranchName { get; } = branchName;

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            skip: false,
            cancellationToken);
    }
}
