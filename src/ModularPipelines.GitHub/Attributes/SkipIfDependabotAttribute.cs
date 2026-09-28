using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfDependabotAttribute : RunConditionAttribute
{
    public SkipIfDependabotAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    public override string ConditionNames => nameof(SkipIfDependabotAttribute);

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        var isDependabot = pipelineContext.Services.GetRequiredService<IGitHubEnvironmentVariables>()?.Actor == "dependabot[bot]";

        return Task.FromResult(isDependabot);
    }
}
