using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;

namespace ModularPipelines.Build.Attributes;

#pragma warning disable CS0618 // This compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipOnMainBranch : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override async Task<bool> Condition(IPipelineContext pipelineContext)
    {
        var repositoryInfo = await pipelineContext.Git().Information.GetInfoAsync().ConfigureAwait(false);
        return repositoryInfo?.BranchName != "main";
    }
}
#pragma warning restore CS0618
