using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Helpers;

/// <summary>
/// Abstraction for displaying module execution progress.
/// Separates the display mechanism from the notification handling logic.
/// </summary>
internal interface IProgressDisplay
{
    /// <summary>
    /// Starts the progress display and keeps it running until completion or cancellation.
    /// </summary>
    /// <param name="organizedModules">The modules to track progress for.</param>
    /// <param name="cancellationToken">Token to signal cancellation.</param>
    Task RunAsync(OrganizedModules organizedModules, CancellationToken cancellationToken);

    /// <summary>
    /// Registers a module as started and begins tracking its progress.
    /// </summary>
    /// <param name="moduleState">The state of the started module.</param>
    /// <param name="estimatedDuration">Estimated time for the module to complete.</param>
    void OnModuleStarted(ModuleState moduleState, TimeSpan estimatedDuration);

    /// <summary>
    /// Updates display when a module completes execution.
    /// </summary>
    /// <param name="moduleState">The state of the completed module.</param>
    /// <param name="isSuccessful">Whether the module completed successfully.</param>
    void OnModuleCompleted(ModuleState moduleState, bool isSuccessful);

    /// <summary>
    /// Updates display when a module is skipped.
    /// </summary>
    /// <param name="moduleState">The state of the skipped module.</param>
    void OnModuleSkipped(ModuleState moduleState);

    /// <summary>
    /// Registers a sub-module as created and begins tracking its progress.
    /// </summary>
    /// <param name="parentModule">The parent module that created the sub-module.</param>
    /// <param name="subModule">The created sub-module.</param>
    /// <param name="estimatedDuration">Estimated time for the sub-module to complete.</param>
    void OnSubModuleCreated(IModule parentModule, SubModuleBase subModule, TimeSpan estimatedDuration);

    /// <summary>
    /// Updates display when a sub-module completes.
    /// </summary>
    /// <param name="subModule">The completed sub-module.</param>
    /// <param name="isSuccessful">Whether the sub-module completed successfully.</param>
    void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful);
}
