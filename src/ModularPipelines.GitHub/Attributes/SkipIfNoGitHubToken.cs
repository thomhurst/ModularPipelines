using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

#pragma warning disable CS0618 // This public compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipIfNoGitHubToken : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var token = pipelineContext.Environment.Variables.GetEnvironmentVariable("GITHUB_TOKEN");

        return Task.FromResult(!string.IsNullOrEmpty(token));
    }
}
#pragma warning restore CS0618
