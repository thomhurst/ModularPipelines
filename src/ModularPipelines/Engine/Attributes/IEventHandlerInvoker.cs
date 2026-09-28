using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes event handlers with consistent error handling.
/// </summary>
internal interface IEventHandlerInvoker
{
    Task InvokePipelineStartHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        CancellationToken cancellationToken);

    Task InvokePipelineEndHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        PipelineSummary summary,
        CancellationToken cancellationToken);

    Task InvokeRegistrationHandlersAsync(
        IEnumerable<IModuleRegistrationHandler> handlers,
        IModuleRegistrationContext context,
        CancellationToken cancellationToken);

    Task InvokeReadyHandlersAsync(
        IEnumerable<IModuleReadyHandler> handlers,
        IModuleHookContext context,
        CancellationToken cancellationToken);

    Task InvokeStartHandlersAsync(
        IEnumerable<IModuleStartHandler> handlers,
        IModuleHookContext context,
        CancellationToken cancellationToken);

    Task InvokeEndHandlersAsync(
        IEnumerable<IModuleEndHandler> handlers,
        IModuleHookContext context,
        IModuleResult result,
        CancellationToken cancellationToken);

    Task InvokeFailureHandlersAsync(
        IEnumerable<IModuleFailureHandler> handlers,
        IModuleHookContext context,
        Exception exception,
        CancellationToken cancellationToken);

    Task InvokeSkippedHandlersAsync(
        IEnumerable<IModuleSkippedHandler> handlers,
        IModuleHookContext context,
        SkipDecision reason,
        CancellationToken cancellationToken);
}
