using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules;

public class GitLastCommitModule : IModule<GitCommit?>
{
    /// <inheritdoc/>
    public async Task<GitCommit?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        context.Logger.LogInformation("Getting Last Git Commit");

        var lastCommit = context.Git().Information.PreviousCommit;

        return lastCommit;
    }
}