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
        string producerModuleTypeName,
        string repositoryRoot,
        CancellationToken cancellationToken)
    {
        if (!IsEnabled)
        {
            return Task.CompletedTask;
        }

        Task restoreTask;
        lock (_restoreLock)
        {
            restoreTask = _restoreTask ??= artifacts.DownloadAsync(
                producerModuleTypeName,
                "build-output",
                repositoryRoot,
                applicationLifetime.ApplicationStopping);
        }

        return restoreTask.WaitAsync(cancellationToken);
    }
}
