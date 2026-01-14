using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

public class SkipIfDependabotAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var isDependabot = pipelineContext.Services.Get<IGitHubEnvironmentVariables>()?.Actor == "dependabot[bot]";

        return Task.FromResult(!isDependabot);
    }
}