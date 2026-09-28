using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoGitHubToken : RunConditionAttribute
{
    public SkipIfNoGitHubToken()
        : base(ConditionIntent.Skip)
    {
    }

    public override string ConditionNames => nameof(SkipIfNoGitHubToken);

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        var token = pipelineContext.Environment.Variables.Get("GITHUB_TOKEN");

        return Task.FromResult(string.IsNullOrEmpty(token));
    }
}
