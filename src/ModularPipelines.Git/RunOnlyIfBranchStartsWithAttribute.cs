using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Requires a module to run on a branch with the specified prefix.
/// </summary>
/// <remarks>
/// Each instance is an independent AND requirement, including alongside grouped RunIfBranch and RunIfBranchStartsWith alternatives. Prefix matching is case-sensitive and ordinal.
/// </remarks>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunOnlyIfBranchStartsWithAttribute(string branchNamePrefix) : RunConditionAttribute(ConditionIntent.Run)
{
    public override string ConditionNames => $"{nameof(RunOnlyIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; } = branchNamePrefix;

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            cancellationToken);
    }
}
