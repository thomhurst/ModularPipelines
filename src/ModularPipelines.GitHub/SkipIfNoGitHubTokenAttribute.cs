using Microsoft.Extensions.Options;
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
        var configuredToken = pipelineContext.Services.GetService<IOptions<GitHubOptions>>()?.Value.AccessToken;
        var token = GitHubAccessToken.Resolve(configuredToken, pipelineContext.Environment.Variables.Get("GITHUB_TOKEN"));

        return Task.FromResult(token is null);
    }
}
