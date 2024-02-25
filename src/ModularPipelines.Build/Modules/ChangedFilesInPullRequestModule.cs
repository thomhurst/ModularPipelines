using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Modules;
using Octokit;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[RunOnLinux]
[SkipOnMainBranch]
public class ChangedFilesInPullRequestModule : Module<IReadOnlyList<File>>
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private readonly GitHubClient _gitHubClient;

    public ChangedFilesInPullRequestModule(IOptions<GitHubSettings> githubSettings, GitHubClient gitHubClient)
    {
        _githubSettings = githubSettings;
        _gitHubClient = gitHubClient;
    }

    protected override async Task<IReadOnlyList<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var result = await context.Git().Commands.Diff(new GitDiffOptions()
        {
            NameOnly = true,
            Arguments = ["origin/main"],
        }, cancellationToken);

        return result.StandardOutput.Split(Environment.NewLine,
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(x => new File(Path.Combine(context.Git().RootDirectory, x)))
            .ToList();
    }
}