using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Modules;
using Octokit;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[RunOnLinuxOnly]
public class DependabotCommitsModule : Module<List<string>>
{
    protected override async Task<List<string>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = context.GitHub().RepositoryInfo;
        
        var latestRelease = await context.GitHub().Client.Repository.Release.GetLatest(repositoryInfo.Owner, repositoryInfo.RepositoryName);

        var commitsSinceRelease = await context.GitHub().Client.Repository.Commit.GetAll(repositoryInfo.Owner,
            repositoryInfo.RepositoryName, new CommitRequest
            {
                Sha = "main",
                Since = latestRelease.CreatedAt.AddMinutes(-2),
                Author = "dependabot[bot]",
            });
        
        var commits = commitsSinceRelease
            .Select(x => x.Commit.Message)
            .ToList();
        
        context.Logger.LogInformation("Commits: {Commits}", string.Join(Environment.NewLine, commits));

        return commits;
    }
}