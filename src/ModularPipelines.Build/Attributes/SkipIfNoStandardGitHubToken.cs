using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoStandardGitHubToken : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipIfNoStandardGitHubToken);

    public Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        var options = pipelineContext.Services.GetRequiredService<IOptions<GitHubSettings>>();

        return Task.FromResult(string.IsNullOrEmpty(options?.Value.StandardToken));
    }
}
