using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Events;

/// <summary>
/// Handles lifecycle events for every module in a pipeline.
/// </summary>
/// <remarks>
/// Register implementations with <c>AddModuleEventHandler&lt;THandler&gt;()</c>.
/// Implement only the callbacks the handler needs; unimplemented callbacks do nothing.
/// </remarks>
public interface IModuleEventHandler :
    IModuleReadyHandler,
    IModuleStartHandler,
    IModuleEndHandler,
    IModuleFailureHandler,
    IModuleSkippedHandler
{
    Task IModuleReadyHandler.OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    Task IModuleStartHandler.OnModuleStartAsync(IModuleHookContext context, CancellationToken cancellationToken) =>
        Task.CompletedTask;

    Task IModuleEndHandler.OnModuleEndAsync(
        IModuleHookContext context,
        IModuleResult result,
        CancellationToken cancellationToken) => Task.CompletedTask;

    Task IModuleFailureHandler.OnModuleFailureAsync(
        IModuleHookContext context,
        Exception exception,
        CancellationToken cancellationToken) => Task.CompletedTask;

    Task IModuleSkippedHandler.OnModuleSkippedAsync(
        IModuleHookContext context,
        SkipDecision reason,
        CancellationToken cancellationToken) => Task.CompletedTask;
}
