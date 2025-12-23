using ModularPipelines.Attributes.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes attribute event receivers with error handling.
/// </summary>
internal interface IAttributeEventInvoker
{
    Task InvokeRegistrationReceiversAsync(IEnumerable<IModuleRegistrationEventReceiver> receivers, IModuleRegistrationContext context);

    Task InvokeStartReceiversAsync(IEnumerable<IModuleStartEventReceiver> receivers, IModuleEventContext context);

    Task InvokeEndReceiversAsync(IEnumerable<IModuleEndEventReceiver> receivers, IModuleEventContext context, ModuleResult result);

    Task InvokeFailureReceiversAsync(IEnumerable<IModuleFailureEventReceiver> receivers, IModuleEventContext context, Exception exception);

    Task InvokeSkippedReceiversAsync(IEnumerable<IModuleSkippedEventReceiver> receivers, IModuleEventContext context, SkipDecision reason);
}
