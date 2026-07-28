using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.GitHub.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoGitHubToken : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipIfNoGitHubToken);

    public Task<bool> EvaluateAsync(IPipelineHookContext pipelineContext)
    {
        var token = pipelineContext.Environment.Variables.GetEnvironmentVariable("GITHUB_TOKEN");

        return Task.FromResult(string.IsNullOrEmpty(token));
    }
}
