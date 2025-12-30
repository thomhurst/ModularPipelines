using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.GitHub.Extensions;

namespace ModularPipelines.Build.Attributes;

public class SkipIfDependencyPullRequest : MandatoryRunConditionAttribute
{
    public override async Task<bool> Condition(IPipelineHookContext pipelineContext)
    {
        var gitHubEnvironmentVariables = pipelineContext.GitHub().EnvironmentVariables;

        if (gitHubEnvironmentVariables.EventName != "pull_request")
        {
            return true;
        }

        var refNamePart = gitHubEnvironmentVariables.RefName?.Split('/').FirstOrDefault();
        if (!int.TryParse(refNamePart, out var prNumber))
        {
            return true;
        }

        if (!long.TryParse(gitHubEnvironmentVariables.RepositoryId, out var repositoryId))
        {
            return true;
        }

        var pr = await pipelineContext.GitHub().Client.PullRequest.Get(repositoryId, prNumber);

        return pr.Labels.All(x => x.Name != "dependencies");
    }
}
