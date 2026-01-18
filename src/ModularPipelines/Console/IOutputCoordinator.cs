namespace ModularPipelines.Console;

/// <summary>
/// Coordinates immediate flushing of module output with synchronization.
/// Ensures modules write output in FIFO completion order without interleaving.
/// </summary>
internal interface IOutputCoordinator
{
    /// <summary>
    /// Gets whether output flushing is currently in progress.
    /// When true, console writes should bypass buffering and go directly to the real console.
    /// </summary>
    bool IsFlushing { get; }

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

    /// <summary>
    /// Sets whether live progress is currently active.
    /// When active, module output is deferred until pipeline end.
    /// </summary>
    /// <param name="isActive">True when progress display is running.</param>
    void SetProgressActive(bool isActive);

    /// <summary>
    /// Called when a module completes. Decides whether to flush immediately or defer.
    /// </summary>
    /// <param name="buffer">The module's output buffer.</param>
    /// <param name="moduleType">The type of the completed module.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task OnModuleCompletedAsync(IModuleOutputBuffer buffer, Type moduleType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Flushes all deferred output in module completion order.
    /// Called after progress ends, before results are printed.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task FlushDeferredAsync(CancellationToken cancellationToken = default);
}
