using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Models;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitLastCommitModule : Module<GitCommit?>
{
    protected override async Task<GitCommit?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        context.Logger.LogInformation("Getting Last Git Commit");

        var lastCommit = context.Git().Information.PreviousCommit;

        var allCommits = await context.Git().Information.Commits(cancellationToken: cancellationToken).ToListAsync(cancellationToken: cancellationToken);

        return lastCommit;
    }
}
