using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfDependabotAttribute : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipIfDependabotAttribute);

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        var isDependabot = pipelineContext.Services.GetRequiredService<IGitHubEnvironmentVariables>()?.Actor == "dependabot[bot]";

        return Task.FromResult(isDependabot);
    }
}
