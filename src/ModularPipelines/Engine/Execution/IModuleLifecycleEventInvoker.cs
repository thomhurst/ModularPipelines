using ModularPipelines.Models;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for invoking module lifecycle events (Ready, Start, End, Failed, Skipped).
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
    /// Called when a module completes successfully.
    /// </summary>
    Task InvokeEndEventAsync(ModuleLifecycleContext context, Enums.Status status, IModuleResult result);

    /// <summary>
    /// Invokes the OnModuleFailed lifecycle event.
    /// Called when a module throws an exception.
    /// </summary>
    Task InvokeFailedEventAsync(ModuleLifecycleContext context, Exception exception);

    /// <summary>
    /// Invokes the OnModuleSkipped lifecycle event.
    /// Called when a module is skipped.
    /// </summary>
    Task InvokeSkippedEventAsync(ModuleLifecycleContext context, Enums.Status status, SkipDecision skipReason);
}
