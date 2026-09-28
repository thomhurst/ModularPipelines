using Microsoft.Extensions.Options;
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfNoStandardGitHubToken : RunConditionAttribute
{
    public SkipIfNoStandardGitHubToken()
        : base(ConditionIntent.Skip)
    {
    }

    public override string ConditionNames => nameof(SkipIfNoStandardGitHubToken);

    public override Task<bool> EvaluateAsync(IPipelineContext pipelineContext, CancellationToken cancellationToken)
    {
        var options = pipelineContext.Services.GetRequiredService<IOptions<GitHubSettings>>();

        return Task.FromResult(string.IsNullOrEmpty(options?.Value.StandardToken));
    }
}
