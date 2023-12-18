using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

[ExcludeFromCodeCoverage]
public class RunIfBranchStartsWithAttribute : RunConditionAttribute
{
    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }

    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName?.StartsWith(BranchNamePrefix) ?? false);
    }
}