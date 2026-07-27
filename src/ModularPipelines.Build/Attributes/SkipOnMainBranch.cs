using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Build.Attributes;

#pragma warning disable CS0618 // This compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipOnMainBranch : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        return Task.FromResult(pipelineContext.Git().Information.BranchName != "main");
    }
}
#pragma warning restore CS0618
