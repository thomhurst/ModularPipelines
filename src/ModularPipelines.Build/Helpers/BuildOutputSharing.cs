using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Distributed;

namespace ModularPipelines.Build.Helpers;

/// <summary>
/// Shares one build-output restore per distributed pipeline process.
/// Standalone runs use the existing checkout without copying or archiving it.
/// </summary>
internal sealed class BuildOutputSharing(IOptions<DistributedOptions> options, IHostApplicationLifetime applicationLifetime)
{
    private readonly Lock _restoreLock = new();
    private Task? _restoreTask;

    /// <summary>
    /// Gets whether build outputs must be shared between pipeline instances.
    /// </summary>
    public bool IsEnabled => options.Value.TotalInstances > 1;

    /// <summary>
    /// Restores build outputs once, sharing the download and outcome with every consumer.
    /// </summary>
    /// <remarks>
    /// The first consumer supplies the download arguments. Each consumer can cancel its own
    /// wait without canceling the shared download. Application shutdown cancels that download.
    /// Download failures remain cached for the lifetime of this instance.
    /// </remarks>
    public Task RestoreAsync(
        IArtifactContext artifacts,
        ModuleId producerModuleId,
        string repositoryRoot,
        CancellationToken cancellationToken)
    {
        // Instance 0 produces these files locally; only workers need to restore them.
        if (!IsEnabled || options.Value.InstanceIndex == 0)
        {
            return Task.CompletedTask;
        }

        Task restoreTask;
        lock (_restoreLock)
        {
            restoreTask = _restoreTask ??= DownloadAndRestoreAsync(artifacts, producerModuleId, repositoryRoot);
        }

        return restoreTask.WaitAsync(cancellationToken);
    }

    private async Task DownloadAndRestoreAsync(IArtifactContext artifacts, ModuleId producerModuleId, string repositoryRoot)
    {
        var archivePath = Path.Combine(repositoryRoot, BuildOutputArchive.FileName);
        try
        {
            await artifacts.DownloadAsync(producerModuleId, "build-output", archivePath, applicationLifetime.ApplicationStopping).ConfigureAwait(false);
            await BuildOutputArchive.RestoreAsync(repositoryRoot, applicationLifetime.ApplicationStopping).ConfigureAwait(false);
        }
        finally
        {
            File.Delete(archivePath);
        }
    }
}
