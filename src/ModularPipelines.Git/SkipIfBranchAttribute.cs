using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Skips a module when its branch matches the specified branch.
/// </summary>
/// <remarks>
/// A satisfied skip condition takes precedence over run conditions.
/// </remarks>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfBranchAttribute(string branchName) : RunConditionAttribute(ConditionIntent.Skip)
{
    public override string ConditionNames => $"{nameof(SkipIfBranchAttribute)}({BranchName})";

    public string BranchName { get; } = branchName;

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            skip: true,
            cancellationToken);
    }
}
