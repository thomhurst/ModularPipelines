namespace ModularPipelines.Distributed;

/// <summary>
/// Opts a distributed backend into deleting the data it stored for a pipeline run once the run ends.
/// </summary>
/// <remarks>
/// <para>
/// Implement this interface on a coordinator, artifact store or master discovery service, or register
/// any other service as <see cref="IDistributedRunCleanup"/>. Only the master of a distributed run calls it.
/// Workers never call it, and neither do pipelines that do not enable distributed mode, because separate local
/// runs can share a run identifier and its data.
/// </para>
/// <para>
/// The call happens once, after the pipeline end handlers and the run report have finished, so every
/// worker that published its final status has stopped using the run's data. A worker that never published
/// one, for example because it crashed, can still be running. <see cref="DistributedOptions.RunCleanup"/>
/// selects which outcomes trigger cleanup and <see cref="DistributedOptions.RunCleanupTimeout"/> bounds
/// each call.
/// </para>
/// <para>
/// Cleanup is best effort. A failure or timeout is logged as a warning and never changes the pipeline
/// result. Implementations should delete only data belonging to <see cref="DistributedRunCleanupContext.RunId"/>
/// and should leave data shared across runs, such as module caches, in place. Follows the evolution policy
/// described on <see cref="IDistributedWorkerCoordinator"/>.
/// </para>
/// </remarks>
public interface IDistributedRunCleanup
{
    /// <summary>
    /// Deletes the data this backend stored for the run.
    /// </summary>
    /// <param name="context">Describes the run that ended.</param>
    /// <param name="cancellationToken">Canceled when <see cref="DistributedOptions.RunCleanupTimeout"/> elapses.</param>
    /// <returns>A task that completes when the run's data is deleted.</returns>
    Task CleanupRunAsync(DistributedRunCleanupContext context, CancellationToken cancellationToken);
}
