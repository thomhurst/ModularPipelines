using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event handlers with error handling.
/// </summary>
internal interface IAttributeEventInvoker
{
    Task InvokeRegistrationReceiversAsync(IEnumerable<IModuleRegistrationEventReceiver> receivers, IModuleRegistrationContext context);

    Task InvokeReadyHandlersAsync(IEnumerable<IModuleReadyHandler> handlers, IModuleHookContext context);

    Task InvokeStartHandlersAsync(IEnumerable<IModuleStartHandler> handlers, IModuleHookContext context);

    Task InvokeEndHandlersAsync(IEnumerable<IModuleEndHandler> handlers, IModuleHookContext context, IModuleResult result);

    Task InvokeFailureHandlersAsync(IEnumerable<IModuleFailureHandler> handlers, IModuleHookContext context, Exception exception);

    Task InvokeSkippedHandlersAsync(IEnumerable<IModuleSkippedHandler> handlers, IModuleHookContext context, SkipDecision reason);
}
