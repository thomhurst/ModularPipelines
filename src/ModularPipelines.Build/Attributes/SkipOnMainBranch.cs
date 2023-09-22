using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Build.Attributes;

public class SkipOnMainBranch : MandatoryRunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName != "main");
    }
}