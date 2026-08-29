using ModularPipelines.Attributes;
using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Build.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class SkipIfDependencyPullRequest : Attribute, IConditionAttribute
{
    public ConditionLogic Logic => ConditionLogic.Skip;

    public string ConditionNames => nameof(SkipIfDependencyPullRequest);

    public async Task<bool> EvaluateAsync(IPipelineContext pipelineContext)
    {
        var gitHubEnvironmentVariables = pipelineContext.Tools.GitHub.EnvironmentVariables;

        if (gitHubEnvironmentVariables.EventName != "pull_request")
        {
            return false;
        }

        var refNamePart = gitHubEnvironmentVariables.RefName?.Split('/').FirstOrDefault();
        if (!int.TryParse(refNamePart, out var prNumber))
        {
            return false;
        }

        if (!long.TryParse(gitHubEnvironmentVariables.RepositoryId, out var repositoryId))
        {
            return false;
        }

        var pr = await pipelineContext.Tools.GitHub.Client.PullRequest.Get(repositoryId, prNumber);

        return pr.Labels.Any(x => x.Name == "dependencies");
    }
}
