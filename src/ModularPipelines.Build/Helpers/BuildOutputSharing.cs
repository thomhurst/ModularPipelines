using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Build.Helpers;

// One instance per pipeline process: distributed test modules share one restore;
// standalone tests use the existing checkout without copying or archiving it.
internal sealed class BuildOutputSharing(IOptions<DistributedOptions> options)
{
    private readonly object _restoreLock = new();
    private Task? _restoreTask;

    public bool IsEnabled => options.Value.TotalInstances > 1;

    public Task RestoreAsync(
        IArtifactContext artifacts,
        string producerModuleTypeName,
        string repositoryRoot,
        CancellationToken cancellationToken)
    {
        if (!IsEnabled)
        {
            return Task.CompletedTask;
        }

        lock (_restoreLock)
        {
            return _restoreTask ??= artifacts.DownloadAsync(
                producerModuleTypeName,
                "build-output",
                repositoryRoot,
                cancellationToken);
        }
    }
}
