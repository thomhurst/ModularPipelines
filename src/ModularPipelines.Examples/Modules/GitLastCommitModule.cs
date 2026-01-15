using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitLastCommitModule : Module<GitCommit?>
{
    /// <inheritdoc/>
    protected override Task<GitCommit?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Getting Last Git Commit");

        var lastCommit = context.Git().Information.PreviousCommit;

        return Task.FromResult(lastCommit);
    }
}