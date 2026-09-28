using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunIfBranchStartsWithAttribute : RunConditionAttribute
{
    public override Type? GroupKey => typeof(BranchConditionHelper);

    public override string ConditionNames => $"{nameof(RunIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
        : base(ConditionIntent.Run)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}",
            cancellationToken);
    }
}
