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

        var prNumber = int.Parse(gitHubEnvironmentVariables.RefName!.Split('/').First());

        var pr = await pipelineContext.GitHub().Client.PullRequest.Get(int.Parse(gitHubEnvironmentVariables.RepositoryId!), prNumber);
        
        return pr.Labels.All(x => x.Name != "dependencies");
    }
}