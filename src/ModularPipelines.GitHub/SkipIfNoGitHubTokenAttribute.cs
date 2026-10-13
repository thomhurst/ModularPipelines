using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoGitHubTokenAttribute : RunConditionAttribute
{
    public SkipIfNoGitHubTokenAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    public override string ConditionNames => nameof(SkipIfNoGitHubTokenAttribute);

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        var token = pipelineContext.Environment.Variables.Get("GITHUB_TOKEN");

        return Task.FromResult(string.IsNullOrEmpty(token));
    }
}
