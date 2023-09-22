using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

public class SkipIfDependabotAttribute : MandatoryRunConditionAttribute
{
    public override Task<bool> Condition(IPipelineContext pipelineContext)
    {
        var isDependabot = pipelineContext.Environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_ACTOR") == "dependabot[bot]";
        
        return Task.FromResult(!isDependabot);
    }
}