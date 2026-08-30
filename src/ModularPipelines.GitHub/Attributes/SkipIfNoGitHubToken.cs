using ModularPipelines.Attributes;
using ModularPipelines;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoGitHubToken : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipIfNoGitHubToken);

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        var token = pipelineContext.Environment.Variables.Get("GITHUB_TOKEN");

        return Task.FromResult(string.IsNullOrEmpty(token));
    }
}
