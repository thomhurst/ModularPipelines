using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class RunOnlyIfBranchStartsWithAttribute : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.All;

    public string ConditionNames => $"{nameof(RunOnlyIfBranchStartsWithAttribute)}({BranchNamePrefix})";

    public string BranchNamePrefix { get; }

    public RunOnlyIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return EvaluateAsync(pipelineContext, default);
    }

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        return BranchConditionHelper.CheckBranchStartsWith(
            pipelineContext,
            BranchNamePrefix,
            "Current Branch: {CurrentBranch} | Can run if starts with: {ExpectedPrefix}",
            cancellationToken);
    }
}
