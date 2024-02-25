using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

public class SkipIfNoStandardGitHubToken : MandatoryRunConditionAttribute
{
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var options = pipelineContext.Get<IOptions<GitHubSettings>>();

        return Task.FromResult(!string.IsNullOrEmpty(options?.Value.StandardToken));
    }
}