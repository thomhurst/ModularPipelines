using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

#pragma warning disable CS0618 // This public compatibility attribute intentionally uses the legacy run-condition contract.
public class SkipIfDependabotAttribute : MandatoryRunConditionAttribute
{
    /// <inheritdoc/>
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        var isDependabot = pipelineContext.Services.Get<IGitHubEnvironmentVariables>()?.Actor == "dependabot[bot]";

        return Task.FromResult(!isDependabot);
    }
}
#pragma warning restore CS0618
