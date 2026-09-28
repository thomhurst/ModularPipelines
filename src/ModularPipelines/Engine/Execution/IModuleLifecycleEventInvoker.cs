using ModularPipelines.Models;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for invoking attribute module lifecycle events (Ready, Start, End, Failed, Skipped).
/// </summary>
internal interface IModuleLifecycleEventInvoker
{
    /// <summary>
    /// Invokes the OnModuleReady lifecycle event.
    /// Called when a module's dependencies are satisfied and it's about to execute.
    /// </summary>
    Task InvokeReadyEventAsync(ModuleLifecycleContext context);

    /// <summary>
    /// Invokes the OnModuleStart lifecycle event.
    /// Called when a module begins execution.
    /// </summary>
    Task InvokeStartEventAsync(ModuleLifecycleContext context);

    /// <summary>
    /// Invokes the OnModuleEnd lifecycle event.
    /// Called when a module completes without being skipped.
    /// </summary>
    Task InvokeEndEventAsync(ModuleLifecycleContext context, IModuleResult result, CancellationToken cancellationToken);

    /// <summary>
    /// Invokes the OnModuleFailed lifecycle event.
    /// Called when a module throws an exception.
    /// </summary>
    Task InvokeFailedEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        Exception exception,
        CancellationToken cancellationToken);

    /// <summary>
    /// Invokes the OnModuleSkipped lifecycle event.
    /// Called when a module is skipped.
    /// </summary>
    Task InvokeSkippedEventAsync(
        ModuleLifecycleContext context,
        IModuleResult result,
        SkipDecision skipReason,
        CancellationToken cancellationToken);
}
