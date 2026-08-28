using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Attributes;

/// <summary>
/// Invokes event handlers with configurable error handling.
/// </summary>
internal class EventHandlerInvoker : IEventHandlerInvoker
{
    private readonly ILogger<EventHandlerInvoker> _logger;

    public EventHandlerInvoker(ILogger<EventHandlerInvoker> logger)
    {
        _logger = logger;
    }

    public Task InvokePipelineStartHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context) =>
        InvokeHandlersAsync(handlers, handler => handler.OnPipelineStartAsync(context), "Pipeline start");

    public Task InvokePipelineEndHandlersAsync(
        IEnumerable<IPipelineEventHandler> handlers,
        IPipelineContext context,
        PipelineSummary summary) =>
        InvokeHandlersAsync(handlers, handler => handler.OnPipelineEndAsync(context, summary), "Pipeline end");

    public Task InvokeRegistrationHandlersAsync(
        IEnumerable<IModuleRegistrationHandler> handlers,
        IModuleRegistrationContext context) =>
        InvokeHandlersAsync(handlers, handler => handler.OnRegistrationAsync(context), "Registration");

    public Task InvokeReadyHandlersAsync(
        IEnumerable<IModuleReadyHandler> handlers,
        IModuleHookContext context) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleReadyAsync(context), "Ready");

    public Task InvokeStartHandlersAsync(
        IEnumerable<IModuleStartHandler> handlers,
        IModuleHookContext context) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleStartAsync(context), "Start");

    public Task InvokeEndHandlersAsync(
        IEnumerable<IModuleEndHandler> handlers,
        IModuleHookContext context,
        IModuleResult result) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleEndAsync(context, result), "End");

    public Task InvokeFailureHandlersAsync(
        IEnumerable<IModuleFailureHandler> handlers,
        IModuleHookContext context,
        Exception exception) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleFailureAsync(context, exception), "Failure");

    public Task InvokeSkippedHandlersAsync(
        IEnumerable<IModuleSkippedHandler> handlers,
        IModuleHookContext context,
        SkipDecision reason) =>
        InvokeHandlersAsync(handlers, handler => handler.OnModuleSkippedAsync(context, reason), "Skipped");

    private async Task InvokeHandlersAsync<THandler>(
        IEnumerable<THandler> handlers,
        Func<THandler, Task> invoke,
        string eventName)
        where THandler : IEventHandler
    {
        foreach (var handler in handlers)
        {
            try
            {
                await invoke(handler).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (handler.ContinueOnError)
                {
                    _logger.LogWarning(
                        ex,
                        "{EventName} handler {Type} failed, continuing",
                        eventName,
                        handler.GetType().Name);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
