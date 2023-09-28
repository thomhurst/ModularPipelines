using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

public class SkipIfDependabotAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        var isDependabot = pipelineContext.Get<IGitHubEnvironmentVariables>()?.Actor == "dependabot[bot]";

        return Task.FromResult(!isDependabot);
    }
}