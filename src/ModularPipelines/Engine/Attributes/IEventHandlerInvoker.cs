using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes event handlers with consistent error handling.
/// </summary>
internal interface IEventHandlerInvoker
{
    Task InvokePipelineStartHandlersAsync(IEnumerable<IPipelineEventHandler> handlers, IPipelineContext context);

    Task InvokePipelineEndHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        PipelineSummary summary);

    Task InvokeRegistrationHandlersAsync(IEnumerable<IModuleRegistrationHandler> handlers, IModuleRegistrationContext context);

    Task InvokeReadyHandlersAsync(IEnumerable<IModuleReadyHandler> handlers, IModuleHookContext context);

    Task InvokeStartHandlersAsync(IEnumerable<IModuleStartHandler> handlers, IModuleHookContext context);

    Task InvokeEndHandlersAsync(IEnumerable<IModuleEndHandler> handlers, IModuleHookContext context, IModuleResult result);

    Task InvokeFailureHandlersAsync(IEnumerable<IModuleFailureHandler> handlers, IModuleHookContext context, Exception exception);

    Task InvokeSkippedHandlersAsync(IEnumerable<IModuleSkippedHandler> handlers, IModuleHookContext context, SkipDecision reason);
}
