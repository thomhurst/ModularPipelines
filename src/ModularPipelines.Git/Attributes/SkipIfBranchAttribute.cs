using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfBranchAttribute : SkipIfAttribute
{
    public override string ConditionNames => $"{nameof(SkipIfBranchAttribute)}({BranchName})";

    public string BranchName { get; }

    public SkipIfBranchAttribute(string branchName)
    {
        BranchName = branchName;
    }

    public override async Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        return await BranchConditionHelper.CheckBranchMatches(
            pipelineContext,
            BranchName,
            "Current Branch: {CurrentBranch} | Will skip on: {SkipBranch}").ConfigureAwait(false);
    }
}
