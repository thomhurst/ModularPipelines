using ModularPipelines.Engine;

namespace ModularPipelines.Console;

/// <summary>
/// Represents an active progress display session.
/// </summary>
/// <remarks>
/// <para>
/// <b>Lifecycle:</b> The session is active from creation until disposal.
/// During this time, the progress display is rendered and module status updates
/// are reflected in the progress bars.
/// </para>
/// <para>
/// <b>Critical:</b> The session MUST be disposed before flushing module output.
/// Disposing the session ends the progress phase and ensures the progress display
/// has fully stopped before any other console output occurs.
/// </para>
/// <para>
/// <b>Thread Safety:</b> All methods are thread-safe and can be called concurrently
/// from notification handlers running on different threads.
/// </para>
/// </remarks>
internal interface IProgressSession : IAsyncDisposable
{
    /// <summary>
    /// Called when a module starts execution.
    /// Adds a progress bar for the module.
    /// </summary>
    /// <param name="state">The module state.</param>
    /// <param name="estimatedDuration">Estimated duration for progress animation.</param>
    void OnModuleStarted(ModuleState state, TimeSpan estimatedDuration);

    /// <summary>
    /// Called when a module completes execution.
    /// Updates the progress bar to show completion status.
    /// </summary>
    /// <param name="state">The module state.</param>
    /// <param name="isSuccessful">Whether the module completed successfully.</param>
    void OnModuleCompleted(ModuleState state, bool isSuccessful);

    /// <summary>
    /// Called when a module is skipped.
    /// Updates the progress bar to show skipped status.
    /// </summary>
    /// <param name="state">The module state.</param>
    void OnModuleSkipped(ModuleState state);

    /// <summary>
    /// Called when a sub-module is created.
    /// Adds a nested progress bar for the sub-module.
    /// </summary>
    /// <param name="parentModule">The parent module.</param>
    /// <param name="subModule">The sub-module.</param>
    /// <param name="estimatedDuration">Estimated duration for progress animation.</param>
    void OnSubModuleCreated(Modules.IModule parentModule, Modules.SubModuleBase subModule, TimeSpan estimatedDuration);

    /// <summary>
    /// Called when a sub-module completes.
    /// Updates the sub-module progress bar to show completion status.
    /// </summary>
    /// <param name="subModule">The sub-module.</param>
    /// <param name="isSuccessful">Whether the sub-module completed successfully.</param>
    void OnSubModuleCompleted(Modules.SubModuleBase subModule, bool isSuccessful);
}
