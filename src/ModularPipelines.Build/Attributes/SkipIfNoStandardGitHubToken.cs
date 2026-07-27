using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

#pragma warning disable CS0618 // This compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipIfNoStandardGitHubToken : MandatoryRunConditionAttribute
{
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var options = pipelineContext.Services.Get<IOptions<GitHubSettings>>();

        return Task.FromResult(!string.IsNullOrEmpty(options?.Value.StandardToken));
    }
}
#pragma warning restore CS0618
