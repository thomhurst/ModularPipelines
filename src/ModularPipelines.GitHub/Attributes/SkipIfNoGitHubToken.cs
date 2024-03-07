using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

public class SkipIfNoGitHubToken : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var token = pipelineContext.Environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_TOKEN");

        return Task.FromResult(!string.IsNullOrEmpty(token));
    }
}