using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Runs a module when its branch matches one of the grouped branch alternatives.
/// </summary>
/// <remarks>
/// All RunIfBranch and RunIfBranchStartsWith attributes on a module share an OR group. Use RunOnlyOnBranch for an independent AND requirement.
/// </remarks>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchAttribute(string branchName) : RunConditionAttribute(ConditionIntent.Run)
{
    public override Type? GroupKey => typeof(BranchConditionHelper);

    public override string ConditionNames => $"{nameof(RunIfBranchAttribute)}({BranchName})";

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
