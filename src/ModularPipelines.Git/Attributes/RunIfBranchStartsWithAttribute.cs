using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Git.Attributes;

public class RunIfBranchStartsWithAttribute : RunConditionAttribute
{
    public string BranchNamePrefix { get; }

    public RunIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }
    
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName?.StartsWith(BranchNamePrefix) ?? false);
    }
}

public class RunOnlyIfBranchStartsWithAttribute : MandatoryRunConditionAttribute
{
    public string BranchNamePrefix { get; }

    public RunOnlyIfBranchStartsWithAttribute(string branchNamePrefix)
    {
        BranchNamePrefix = branchNamePrefix;
    }
    
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName?.StartsWith(BranchNamePrefix) ?? false);
    }
}