using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Git;

/// <summary>
/// Runs a module when its branch matches one of the grouped branch alternatives by prefix.
/// </summary>
/// <remarks>
/// All RunIfBranch and RunIfBranchStartsWith attributes on a module share an OR group. Prefix matching is case-sensitive and ordinal. Use RunOnlyIfBranchStartsWith for an independent AND requirement.
/// </remarks>
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchStartsWithAttribute(string branchNamePrefix) : RunConditionAttribute(ConditionIntent.Run)
{
    public override Type? GroupKey => typeof(BranchConditionHelper);

    public override string ConditionNames => $"{nameof(RunIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; } = branchNamePrefix;

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            cancellationToken);
    }
}
