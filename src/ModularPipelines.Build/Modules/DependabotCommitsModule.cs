using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnLinuxOnly]
public class DependabotCommitsModule : Module<string>
{
    protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = context.GitHub().RepositoryInfo;
        
        context.Logger.LogInformation("RepositoryInfo: {RepositoryInfo}", repositoryInfo);
        
        var latestRelease = await context.GitHub().Client.Repository.Release.GetLatest(repositoryInfo.Owner, repositoryInfo.RepositoryName);
        
        var sha = latestRelease.TargetCommitish;
        
        context.Logger.LogInformation("Sha: {Sha}", sha);

        var commitsResult = await context.Git().Commands.Log(new GitLogOptions
        {
            Author = "dependabot",
            Format = "%s",
            Arguments = [$"{sha}..HEAD"],
        }, cancellationToken);

        var commits = commitsResult.StandardOutput;
        
        context.Logger.LogInformation("Commits: {Commits}", commits);

        return commits;
    }
}