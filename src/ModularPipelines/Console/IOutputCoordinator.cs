namespace ModularPipelines.Console;

/// <summary>
/// Coordinates immediate flushing of module output with synchronization.
/// Ensures modules write output in FIFO completion order without interleaving.
/// </summary>
internal interface IOutputCoordinator
{
    /// <summary>
    /// Sets the progress controller for pause/resume coordination.
    /// Must be called before any modules complete.
    /// </summary>
    /// <param name="controller">The progress controller.</param>
    void SetProgressController(IProgressController controller);

    /// <summary>
    /// Enqueues a module's buffer for immediate flushing and waits until flushed.
    /// Called when a module's logger is disposed.
    /// </summary>
    /// <param name="buffer">The buffer to flush.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task that completes when the buffer has been flushed.</returns>
    Task EnqueueAndFlushAsync(IModuleOutputBuffer buffer, CancellationToken cancellationToken = default);
}
